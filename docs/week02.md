\# Week 02 - Offline SQLite + Manual Mode



\## Mục tiêu tuần

Xây dựng chức năng sử dụng dữ liệu offline bằng SQLite và hoàn thiện luồng duyệt gian hàng thủ công (manual mode).



\## Những gì đã hoàn thành

\- Tạo cơ sở dữ liệu SQLite local trong mobile app

\- Seed dữ liệu ban đầu từ `seed.json`

\- Lưu các bảng:

&#x20; - Zone

&#x20; - Booth

&#x20; - BoothMenuItem

\- Hoàn thiện luồng:

&#x20; - Gate Mode

&#x20; - Manual mode

&#x20; - Zone list

&#x20; - Booth by Zone

&#x20; - Booth Detail

\- Hiển thị menu theo từng gian hàng

\- Hỗ trợ đổi ngôn ngữ VI / EN



\## Chức năng đã chạy được

\- Xem danh sách gian hàng

\- Chọn khu vực thủ công

\- Xem chi tiết gian hàng theo khu vực

\- Xem menu món ăn

\- Đổi ngôn ngữ Việt / Anh

\- Sử dụng dữ liệu offline không cần internet



\## Minh chứng

\- `assets/screenshots/week02-booth-list.png`

\- `assets/screenshots/week02-zone-list.png`

\- `assets/screenshots/week02-booth-by-zone.png`

\- `assets/screenshots/week02-booth-detail.png`



\## Khó khăn gặp phải

\- Lỗi route khi chuyển giữa Zone → Booth → Detail

\- Lỗi `Content is set more than once` trong XAML

\- Lỗi `MenuItem` bị trùng với `Microsoft.Maui.Controls.MenuItem`



\## Cách xử lý

\- Tạo route riêng cho từng page trong `AppShell`

\- Dùng `Grid` thay vì đặt nhiều content cùng cấp

\- Đổi `MenuItem` model thành `BoothMenuItem`



\## Kế hoạch tuần 3

\- Tích hợp MAUI Maps

\- Xin quyền GPS

\- Hiển thị vị trí hiện tại

\- Tính booth gần nhất

