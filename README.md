# \# POI Booth Narration System (MAUI + Offline SQLite + GPS + QR + Web Dashboard)

# 

# \## 1. Giới thiệu

# Hệ thống hỗ trợ \*\*du khách\*\* tham quan khu vực hội chợ/triển lãm gồm nhiều \*\*gian hàng (Booth)\*\*.

# Ứng dụng mobile sẽ \*\*tự động thuyết minh\*\* khi du khách đến gần gian hàng (GPS/Geofence) hoặc khi quét QR.

# Hệ thống hỗ trợ \*\*offline\*\* bằng SQLite, đồng thời có \*\*Web Dashboard\*\* cho Chủ gian hàng và Admin quản lý nội dung + xem thống kê.

# 

# \## 2. Yêu cầu chính (theo đề bài)

# \### A. GPS Tracking (tự động)

# \- Lưu tọa độ GPS của từng gian hàng.

# \- Khi du khách tới gần (ví dụ < 10m), app tự:

# &nbsp; - Hiển thị thông tin gian hàng

# &nbsp; - Phát thuyết minh đa ngôn ngữ (VI/EN)

# 

# \### B. QR Code (2 lựa chọn ở cổng)

# \- Quét QR tại cổng -> hiện 2 lựa chọn:

# &nbsp; 1) Tracking theo GPS (Auto)

# &nbsp; 2) Chọn khu vực thủ công (Manual/ảo) nếu không bật GPS

# 

# \### C. Offline SQLite

# \- Dữ liệu nhỏ (tên gian hàng, menu, mô tả, tọa độ, script thuyết minh) lưu trên thiết bị -> dùng offline.

# \- Dữ liệu lớn (ảnh/audio/video dung lượng cao) lưu trên server -> app tải về khi cần (có cache).

# 

# \### D. Web Dashboard

# \- \*\*Du khách\*\*: dùng Mobile App

# \- \*\*Chủ gian hàng (Owner)\*\*: dùng Web để quản lý menu, mô tả, xem lượt xem/thống kê của gian mình

# \- \*\*Admin\*\*: dùng Web quản lý toàn bộ gian hàng, tài khoản, nội dung

# 

# \### E. Cơ sở hạ tầng

# \- \*\*Server\*\*: Database chính + Media + REST API

# \- \*\*Client\*\*: Mobile App + Web Dashboard

# 

# \## 3. Luồng hoạt động tổng thể

# 1\) Du khách vào khu -> quét QR ở cổng  

# 2\) Chọn:

# &nbsp;  - GPS Tracking (Auto) hoặc

# &nbsp;  - Manual chọn khu vực (Zone)  

# 3\) Khi đến gần gian hàng:

# &nbsp;  - GPS kích hoạt thuyết minh tự động (theo bán kính)

# 4\) Tại gian hàng:

# &nbsp;  - Quét QR booth -> xem menu chi tiết + phát thuyết minh

# 

# \## 4. Tính năng (Checklist)

# \### Mobile App (.NET MAUI: Android + iOS)

# \- \[ ] Quét QR cổng: chọn GPS/Manual

# \- \[ ] Manual mode: Zone -> Booth -> Booth Detail

# \- \[ ] Offline SQLite: lưu Zone/Booth/Menu/Logs

# \- \[ ] Map: pins Booth + vị trí user

# \- \[ ] GPS tracking foreground + tính khoảng cách

# \- \[ ] Geofence trigger + debounce + cooldown chống spam

# \- \[ ] Thuyết minh đa ngôn ngữ (TTS/Audio)

# \- \[ ] Quét QR booth: mở detail + phát + xem menu

# \- \[ ] Đồng bộ dữ liệu (sync) từ server khi online

# 

# \### Server API (ASP.NET Core Web API)

# \- \[ ] CRUD Zone

# \- \[ ] CRUD Booth + quản lý nội dung VI/EN

# \- \[ ] CRUD MenuItem theo Booth

# \- \[ ] Upload/serve media (ảnh/audio/video)

# \- \[ ] Logs (GPS/QR/Manual) + thống kê cơ bản

# \- \[ ] Auth + Roles: Admin / Owner

# 

# \### Web Dashboard (Blazor Server / MVC / React tuỳ chọn)

# \- \[ ] Login/Logout

# \- \[ ] Admin: quản lý tài khoản + booth + zone

# \- \[ ] Owner: quản lý booth của mình + menu + nội dung thuyết minh

# \- \[ ] Thống kê: lượt xem/nghe theo booth, theo ngày, theo trigger

# 

# \## 5. Công nghệ sử dụng

# \- Mobile: .NET MAUI (Android/iOS), MVVM (CommunityToolkit), SQLite (sqlite-net-pcl)

# \- Server: ASP.NET Core Web API + EF Core + SQL Server/PostgreSQL

# \- Web Dashboard: Blazor Server (khuyến nghị để full C#)

# \- QR: ZXing.Net.Maui

# \- TTS: MAUI TextToSpeech (Essentials)

