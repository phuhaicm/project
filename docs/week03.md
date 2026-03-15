\# Week 03 - Map + GPS Foreground + Nearest Booth



\## Mục tiêu tuần

Tích hợp bản đồ và GPS để xác định gian hàng ẩm thực gần nhất trong hội chợ.



\## Những gì đã hoàn thành

\- Tích hợp MAUI Maps

\- Tạo tab Map

\- Hiển thị map trong ứng dụng

\- Tạo `LocationService`

\- Xin quyền vị trí trên Android

\- Lấy vị trí hiện tại của thiết bị/emulator

\- Tính khoảng cách từ user đến từng booth bằng công thức Haversine

\- Xác định gian hàng gần nhất

\- Add booth lên map dưới dạng pins

\- Hiển thị số lượng booth / số lượng pin

\- GPS mode từ Gate chuyển sang tab Map



\## Chức năng đã chạy được

\- Hiển thị vị trí hiện tại

\- Hiển thị gian hàng gần nhất

\- Hiển thị booth lên map

\- Tính khoảng cách theo thời gian thực

\- Test được bằng emulator mock location



\## Minh chứng

\- `assets/screenshots/week03-map.png`

\- `assets/screenshots/week03-gps-location.png`

\- `assets/screenshots/week03-nearest-booth.png`

\- `assets/screenshots/week03-pins.png`



\## Khó khăn gặp phải

\- Lỗi Google Maps API key / map tile

\- Khoảng cách quá xa do emulator dùng location mặc định

\- Pin không hiện do map focus chưa đúng vùng



\## Cách xử lý

\- Bổ sung API key cho Android map

\- Dùng emulator mock location gần booth

\- Điều chỉnh `MoveToRegion()` để focus giữa user và nearest booth



\## Kế hoạch tuần 4

\- Tạo geofence trigger

\- Tự động phát thuyết minh bằng TTS

\- Debounce và cooldown

\- Ghi playback log

