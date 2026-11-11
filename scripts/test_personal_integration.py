"""Development-only end-to-end HTTP/SQL test using disposable records."""
import secrets,html,http.cookiejar,urllib.request,urllib.parse,urllib.error,re,uuid,subprocess
BASE='http://localhost:5269'; RUN=uuid.uuid4().hex[:12]; password='Test!A9'+uuid.uuid4().hex
ids=[]; vehicle=None; booking=None

def sql(q):
 p=subprocess.run(['sqlcmd','-S',r'(localdb)\MSSQLLocalDB','-d','NeighborRide_G08_Dev','-E','-b','-h','-1','-W','-Q','SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET ANSI_PADDING ON; SET ANSI_WARNINGS ON; SET CONCAT_NULL_YIELDS_NULL ON; SET ARITHABORT ON; SET NUMERIC_ROUNDABORT OFF; SET NOCOUNT ON; '+q],capture_output=True,text=True,encoding='utf-8',errors='replace')
 if p.returncode:raise RuntimeError(p.stdout+p.stderr)
 return p.stdout.strip()
class Client:
 def __init__(self):self.opener=urllib.request.build_opener(urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()))
 def req(self,path,form=None):
  req=urllib.request.Request(BASE+path,data=urllib.parse.urlencode(form).encode() if form is not None else None)
  try:
   with self.opener.open(req,timeout=15) as r:return r.status,html.unescape(r.read().decode('utf-8'))
  except urllib.error.HTTPError as e:return e.code,html.unescape(e.read().decode('utf-8'))
 def form(self,path,fields):
  code,body=self.req('/Personal' if path.startswith('/Personal/') else path);assert code==200,(path,code)
  token=re.search(r'name="__RequestVerificationToken" type="hidden" value="([^"]+)"',body)
  assert token,'missing CSRF token';return self.req(path,dict(fields,__RequestVerificationToken=token.group(1)))
try:
 community=sql('SELECT TOP 1 Id FROM Communities ORDER BY Id');category=sql('SELECT TOP 1 Id FROM VehicleCategories ORDER BY Id')
 assert community and category
 clients=[Client(),Client()]
 for i,c in enumerate(clients):
  email=f'g08-test-{RUN}-{i}@example.com'
  code,body=c.form('/Account/Register',dict(FullName='Test '+RUN,Email=email,Phone='0'+str(secrets.randbelow(10**9)).zfill(9),CommunityId=community,Password=password,ConfirmPassword=password))
  uid=sql("SELECT Id FROM AspNetUsers WHERE Email='"+email+"'");assert re.fullmatch(r'[0-9a-f-]{36}',uid);ids.append(uid)
  assert code==200 and 'chờ quản trị viên duyệt' in body
  code,body=c.form('/Account/Login',dict(Email=email,Password=password));assert code==200 and 'chưa được duyệt' in body
  sql("UPDATE AspNetUsers SET MemberStatus=1 WHERE Id='"+uid+"'")
  code,body=c.form('/Account/Login',dict(Email=email,Password=password));assert code==200 and 'Đăng xuất' in body
 print('PASS registration, pending block, login, CSRF forms')
 phone_a='0'+str(secrets.randbelow(10**9)).zfill(9)
 code,body=clients[0].form('/Account/Profile',dict(FullName='Updated '+RUN,Phone=phone_a));assert code==200 and 'Đã cập nhật hồ sơ' in body
 code,body=clients[1].form('/Account/Profile',dict(FullName='Other '+RUN,Phone=phone_a));assert code==200 and 'Số điện thoại đã được sử dụng' in body
 code,body=clients[1].form('/Account/Profile',dict(FullName='Other '+RUN,Phone='123'));assert code==200 and 'Số điện thoại phải gồm 10 chữ số' in body
 assert sql(f"SELECT PhoneNumber FROM AspNetUsers WHERE Id='{ids[0]}'")==phone_a
 print('PASS profile phone persisted, duplicate and invalid rejected')
 vehicle=sql(f"INSERT INTO Vehicles(OwnerId,CategoryId,CommunityId,Name,ShareType,Price,Status) OUTPUT INSERTED.Id VALUES('{ids[1]}',{category},{community},'G08TEST{RUN}',0,0,1)")
 booking=sql(f"INSERT INTO Bookings(VehicleId,BorrowerId,StartAt,EndAt,EstimatedFee,Status,CreatedAt) OUTPUT INSERTED.Id VALUES({vehicle},'{ids[0]}',DATEADD(day,-2,GETUTCDATE()),DATEADD(day,-1,GETUTCDATE()),0,3,GETUTCDATE())")
 code,body=clients[0].form('/Personal/Review',dict(BookingId=booking,Rating=5,Comment='G08TEST'+RUN));assert code==200 and 'Đã lưu đánh giá' in body
 review=sql(f'SELECT Id FROM Reviews WHERE BookingId={booking}');assert review
 code,body=clients[0].form('/Personal/Review',dict(BookingId=booking,Rating=5,Comment='Duplicate'));assert 'đã đánh giá' in body
 code,body=clients[0].form('/Personal/Reply',dict(id=review,Content='Sai quyền phản hồi'));assert code==403
 code,body=clients[1].form('/Personal/Reply',dict(id=review,Content='Cảm ơn đánh giá'));assert 'Đã lưu phản hồi' in body
 code,body=clients[1].form('/Personal/Reply',dict(id=review,Content='Phản hồi thứ hai'));assert 'đã có phản hồi' in body
 print('PASS completed review, duplicate, reply authorization and duplicate')
 code,body=clients[0].form('/Personal/Report',dict(TargetType='Vehicle',TargetId=vehicle,Reason='Test reason',Description='G08TEST'+RUN));assert 'Đã gửi báo cáo' in body
 report=sql(f"SELECT Id FROM ViolationReports WHERE ReporterId='{ids[0]}' AND TargetId='{vehicle}'");assert report
 code,body=clients[0].req('/Moderation');assert code==403
 code,body=clients[0].req('/Personal/Report',dict(TargetType='Vehicle',TargetId=vehicle,Reason='Test reason',Description='G08TEST'+RUN));assert code==400
 print('PASS report persistence, admin authorization, CSRF rejection')
 sql(f"INSERT INTO AspNetUserRoles(UserId,RoleId) SELECT '{ids[1]}',Id FROM AspNetRoles WHERE Name='Admin'")
 admin=Client();admin_email=f'g08-test-{RUN}-1@example.com';code,body=admin.form('/Account/Login',dict(Email=admin_email,Password=password));assert code==200
 code,body=admin.req('/Moderation');assert code==200
 token=re.search(r'name="__RequestVerificationToken" type="hidden" value="([^"]+)"',body).group(1)
 version=re.search(r'name="version" value="([^"]+)"',body).group(1)
 code,body=admin.req('/Moderation/Resolve',dict(id=report,status='Resolved',note='G08TEST '+RUN+' completed',version=version,__RequestVerificationToken=token));assert code==200
 assert sql(f'SELECT Status FROM ViolationReports WHERE Id={report}')=='2'
 code,body=admin.req('/Moderation/Resolve',dict(id=report,status='Resolved',note='G08TEST '+RUN+' again',version=version,__RequestVerificationToken=token));assert code==409
 print('PASS admin report resolution, terminal state and notification')
finally:
 if ids:
  uidlist=','.join("'"+x+"'" for x in ids)
  sql(f"DELETE FROM Notifications WHERE UserId IN({uidlist}); DELETE FROM ViolationReports WHERE ReporterId IN({uidlist}); DELETE FROM Reviews WHERE ReviewerId IN({uidlist}) OR RevieweeId IN({uidlist}); DELETE FROM Bookings WHERE BorrowerId IN({uidlist}); DELETE FROM Vehicles WHERE OwnerId IN({uidlist}); DELETE FROM AspNetUserRoles WHERE UserId IN({uidlist}); DELETE FROM AspNetUsers WHERE Id IN({uidlist});")
  assert sql(f'SELECT COUNT(*) FROM AspNetUsers WHERE Id IN({uidlist})')=='0'
  print('PASS test data cleanup')
