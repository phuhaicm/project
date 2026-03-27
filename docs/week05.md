\# Week 04 - Geofence + TTS + Debounce/Cooldown



\## Mục tiêu tuần

Xây dựng chức năng tự động thuyết minh khi du khách tới gần gian hàng ẩm thực.



\## Những gì đã hoàn thành

\- Tạo `PlaybackLog` để lưu lịch sử phát

\- Tạo `NarrationService` dùng Text-to-Speech

\- Tạo `GeofenceService`

\- Kiểm tra user có đi vào vùng của gian hàng hay không

\- Áp dụng debounce:

&#x20; - user phải đứng trong vùng đủ thời gian mới trigger

\- Áp dụng cooldown:

&#x20; - tránh phát lặp liên tục

\- Tự động mở `BoothDetailPage` khi trigger GPS

\- Tự động đọc mô tả gian hàng bằng TTS

\- Thêm nút manual `Nghe thuyết minh` trong `BoothDetailPage`



\## Chức năng đã chạy được

\- Bấm manual play để nghe thuyết minh

\- Trigger tự động khi user đi vào bán kính booth

\- Không phát lặp liên tục khi đứng yên

\- Ghi log phát vào SQLite

\- Hỗ trợ VI / EN trong TTS



\## Minh chứng

\- `assets/screenshots/week04-map-active.png`

\- `assets/screenshots/week04-booth-detail-triggered.png`

\- `assets/screenshots/week04-play-manual.png`

\- `assets/screenshots/week04-log-or-state.png`



\## Khó khăn gặp phải

\- `BoothDetailPage` điều hướng lúc được lúc không

\- Trigger GPS không chạy do route / debounce / cooldown

\- SQLite giữ dữ liệu cũ khi sửa `seed.json`



\## Cách xử lý

\- Clear `SelectedItem` sau khi chọn booth

\- Tạo route riêng cho `BoothDetailPage`

\- Điều chỉnh geofence logic ổn định hơn

\- Gỡ app để seed lại SQLite với dữ liệu mới



\## Kế hoạch tuần 5

\- QR scan cho booth

\- Ghi log rõ hơn

\- Chuẩn bị tích hợp web/server nếu cần



