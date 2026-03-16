# PRD – Ứng dụng thuyết minh gian hàng ẩm thực tự động trong hội chợ

## 1. Thông tin tài liệu
- **Tên sản phẩm:** Ứng dụng thuyết minh gian hàng ẩm thực tự động trong hội chợ
- **Nền tảng hiện tại:** Android Mobile App (.NET MAUI)
- **Phiên bản PRD:** v1.0
- **Tình trạng hiện tại:** Đã hoàn thành đến Week 04
- **Nhóm thực hiện:** 2 thành viên
- **Người phụ trách phần mobile hiện tại:** Lê Nguyễn Phú Hải

---

## 2. Tổng quan sản phẩm

Ứng dụng hỗ trợ du khách tham quan hội chợ ẩm thực bằng cách:
- xem danh sách gian hàng,
- chọn gian hàng thủ công theo khu vực,
- hoặc để ứng dụng tự nhận biết vị trí hiện tại bằng GPS,
- tự động phát thuyết minh khi du khách đi vào gần gian hàng.

Ứng dụng ưu tiên trải nghiệm:
- **đơn giản**
- **offline-first**
- **không cần đăng nhập**
- **phù hợp môi trường hội chợ đông người**

---

## 3. Bài toán / vấn đề cần giải quyết

Trong hội chợ ẩm thực, du khách thường gặp các vấn đề:
1. Không biết gần mình có những gian hàng nào
2. Không có mô tả rõ về món ăn / đặc trưng gian hàng
3. Không muốn phải tự tìm hiểu từng booth một
4. Không phải lúc nào cũng có internet ổn định tại hội chợ
5. Cần trải nghiệm nhanh, trực quan, phù hợp cả người Việt và khách nước ngoài

Ứng dụng này giải quyết các vấn đề đó bằng:
- dữ liệu SQLite offline,
- bản đồ + GPS,
- geofence,
- thuyết minh tự động bằng TTS,
- hỗ trợ ngôn ngữ Việt / Anh.

---

## 4. Mục tiêu sản phẩm

### 4.1 Mục tiêu chính
- Hỗ trợ du khách khám phá các gian hàng ẩm thực dễ dàng hơn
- Tự động cung cấp thông tin khi du khách đến gần một gian hàng
- Đảm bảo vẫn hoạt động được trong điều kiện internet yếu hoặc không có mạng

### 4.2 Mục tiêu phụ
- Hỗ trợ song ngữ Việt / Anh
- Dễ demo trên Android
- Có thể mở rộng thêm QR scan, analytics hoặc dashboard trong giai đoạn sau

---

## 5. Đối tượng người dùng

### 5.1 Du khách
- Người tham gia hội chợ
- Muốn tìm hiểu nhanh về gian hàng
- Không muốn thao tác phức tạp

### 5.2 Ban tổ chức / giảng viên / người xem demo
- Cần nhìn thấy rõ luồng GPS, geofence, booth detail, narration

---

## 6. Phạm vi sản phẩm

### 6.1 In scope (nằm trong phạm vi hiện tại)
- Mobile app Android bằng .NET MAUI
- SQLite lưu dữ liệu gian hàng offline
- Manual mode:
  - chọn khu vực
  - xem gian hàng theo khu vực
  - xem chi tiết gian hàng
- Song ngữ Việt / Anh
- Tab Map
- GPS foreground
- Tính khoảng cách đến gian hàng
- Xác định gian hàng gần nhất
- Geofence trigger
- TTS narration
- Debounce + Cooldown
- Playback log trong SQLite

### 6.2 Out of scope (chưa nằm trong phạm vi hiện tại)
- Xác thực người dùng
- FaceID / nhận diện khuôn mặt
- Web dashboard
- Backend server hoàn chỉnh
- Payment / voucher
- AI content generation
- iOS release production
- Admin / Owner portal

---

## 7. User Flow chính

### 7.1 Flow 1 – Manual mode
1. Người dùng mở app
2. Vào tab Booths
3. Chọn `Manual Mode`
4. Chọn khu vực (Zone A / Zone B)
5. Xem danh sách booth theo khu vực
6. Chọn booth
7. Xem chi tiết booth
8. Có thể bấm `Nghe thuyết minh`

### 7.2 Flow 2 – GPS mode
1. Người dùng mở app
2. Chọn `GPS Tracking`
3. App xin quyền vị trí
4. App hiển thị Map + vị trí hiện tại
5. App tính booth gần nhất
6. Khi người dùng đi vào vùng geofence của booth:
   - app tự trigger
   - app mở chi tiết booth
   - app tự đọc mô tả

---

## 8. Danh sách tính năng chính

### Tính năng 1 – Điều hướng và cấu trúc màn hình
- Tab Booths
- Tab Map
- GateModePage
- ZoneListPage
- BoothByZonePage
- BoothDetailPage

### Tính năng 2 – SQLite offline
- Lưu Zone
- Lưu Booth
- Lưu BoothMenuItem
- Lưu PlaybackLog
- Seed dữ liệu từ `seed.json`

### Tính năng 3 – Manual Mode
- Chọn khu vực
- Xem booth theo khu vực
- Xem chi tiết booth

### Tính năng 4 – Song ngữ VI / EN
- Đổi ngôn ngữ hiển thị
- Áp dụng cho tên booth, mô tả, menu, narration

### Tính năng 5 – Bản đồ
- Hiển thị map
- Hiển thị vị trí hiện tại
- Hiển thị pins của booth

### Tính năng 6 – GPS foreground
- Xin quyền location
- Lấy vị trí hiện tại
- Theo dõi vị trí liên tục

### Tính năng 7 – Nearest Booth
- Tính khoảng cách user → booth
- Hiển thị booth gần nhất

### Tính năng 8 – Geofence
- Kiểm tra user có đi vào bán kính booth hay không
- Chọn booth phù hợp theo priority + distance

### Tính năng 9 – Narration / TTS
- Phát mô tả booth bằng Text-to-Speech
- Manual play từ BoothDetailPage
- Auto play khi geofence trigger

### Tính năng 10 – Debounce / Cooldown / Playback Log
- Debounce: tránh trigger khi đi lướt ngang
- Cooldown: tránh phát lặp
- Log lại lịch sử đã phát

---

## 9. Yêu cầu chức năng (Functional Requirements)

### FR-01: App phải hiển thị danh sách gian hàng
- Người dùng phải xem được danh sách booth từ dữ liệu SQLite
- Dữ liệu hiển thị được cả khi offline

### FR-02: App phải hỗ trợ chọn booth theo khu vực
- Người dùng phải chọn được Zone A / Zone B
- BoothByZonePage phải hiển thị đúng danh sách booth theo zoneId

### FR-03: App phải hiển thị chi tiết booth
- Khi chọn booth, app phải mở BoothDetailPage
- BoothDetailPage phải hiển thị:
  - tên
  - mô tả
  - menu

### FR-04: App phải hỗ trợ đổi ngôn ngữ
- Tên và mô tả booth phải đổi theo VI / EN
- TTS cũng phải đọc đúng ngôn ngữ đã chọn

### FR-05: App phải xin quyền vị trí trên Android
- Nếu chưa có quyền → yêu cầu người dùng cấp quyền
- Nếu từ chối → app vẫn dùng Manual mode

### FR-06: App phải tính được khoảng cách đến booth
- Dùng tọa độ user và tọa độ booth
- Tính bằng công thức Haversine

### FR-07: App phải hiển thị booth gần nhất
- Trên MapPage phải có label nearest booth
- Khoảng cách phải update theo vị trí mới

### FR-08: App phải phát narration thủ công
- Người dùng bấm nút `Nghe thuyết minh` tại BoothDetailPage
- App đọc mô tả booth bằng TTS

### FR-09: App phải tự động trigger narration khi vào vùng booth
- Nếu khoảng cách user ≤ radiusMeters của booth
- Và đủ debounce
- Thì app phải trigger narration

### FR-10: App phải chống trigger lặp
- Nếu đang đứng trong cùng một vùng booth → không phát liên tục
- Nếu vừa phát xong → phải chờ hết cooldown mới phát lại

### FR-11: App phải lưu log phát
- Mỗi lần narration chạy phải lưu:
  - BoothId
  - TriggerType
  - Language
  - PlayedAtUtc
  - Lat/Lng

---

## 10. Yêu cầu phi chức năng (Non-Functional Requirements)

### NFR-01: Hoạt động offline
- Dữ liệu booth và menu phải dùng được khi không có internet

### NFR-02: Hiệu năng đủ tốt trên Android emulator / thiết bị thật
- GPS và map không được làm app treo
- TTS phải phản hồi trong thời gian hợp lý

### NFR-03: Ổn định điều hướng
- Các route phải nhất quán
- Không crash khi chuyển từ Booth → Detail → Map

### NFR-04: Dễ demo
- Các màn hình phải rõ ràng, dễ giải thích
- Có thể test bằng emulator mock location

### NFR-05: Dễ mở rộng
- Kiến trúc service nên cho phép mở rộng thêm:
  - QR booth
  - analytics
  - server sync

---

## 11. Dữ liệu hệ thống

### 11.1 Zone
- Id
- NameVi
- NameEn
- CenterLat
- CenterLng
- RadiusMeters

### 11.2 Booth
- Id
- ZoneId
- NameVi
- NameEn
- DescVi
- DescEn
- Priority
- Lat
- Lng
- RadiusMeters
- ImageUrl

### 11.3 BoothMenuItem
- Id
- BoothId
- Name
- Description
- Price
- ImageUrl

### 11.4 PlaybackLog
- Id
- BoothId
- TriggerType
- Language
- PlayedAtUtc
- Lat
- Lng
- IsCompleted

---

## 12. Các màn hình chính

1. BoothListPage
2. GateModePage
3. ZoneListPage
4. BoothByZonePage
5. BoothDetailPage
6. MapPage

---

## 13. Tiêu chí hoàn thành (Acceptance Criteria)

### AC-01
Người dùng có thể vào Manual mode và xem được chi tiết booth

### AC-02
App hiển thị được map và vị trí hiện tại

### AC-03
App tính được booth gần nhất theo GPS

### AC-04
Khi user đi vào geofence của booth, app tự trigger narration

### AC-05
App không phát lặp liên tục khi user đứng yên trong cùng booth

### AC-06
App lưu được playback log vào SQLite

### AC-07
Người dùng đổi VI / EN thì narration và nội dung hiển thị thay đổi tương ứng

---

## 14. Kế hoạch triển khai theo tuần

### Week 01
- Setup project
- Shell navigation
- Tạo skeleton pages

### Week 02
- SQLite offline
- Seed dữ liệu
- Manual flow
- VI / EN

### Week 03
- MAUI Maps
- GPS foreground
- Hiển thị nearest booth
- Pins

### Week 04
- Geofence
- TTS narration
- Debounce
- Cooldown
- PlaybackLog

### Week 05 (dự kiến)
- QR booth scan
- Cải thiện logging / analytics
- Ổn định toàn bộ flow

### Week 06 (dự kiến)
- Hoàn thiện báo cáo
- Hoàn thiện demo/video
- Polish UI
- Tổng hợp kết quả

---

## 15. Rủi ro và khó khăn

### Rủi ro kỹ thuật
- Android map tile / API key
- Emulator GPS không ổn định
- Route Shell dễ lỗi nếu register không đúng
- SQLite giữ dữ liệu cũ khi sửa `seed.json`

### Cách xử lý
- Test bằng emulator mock location
- Rebuild + uninstall app khi reseed DB
- Tách service rõ ràng:
  - LocationService
  - GeofenceService
  - NarrationService

---

## 16. Định hướng mở rộng trong tương lai
- QR code tại booth
- Web dashboard cho admin / chủ gian hàng
- Sync dữ liệu từ server
- Thống kê lượt nghe narration
- Hỗ trợ đa ngôn ngữ nâng cao hơn

---

## 17. Kết luận
Đây là một ứng dụng mobile hướng tới trải nghiệm tham quan hội chợ ẩm thực thông minh, tập trung vào:
- offline-first,
- GPS + geofence,
- thuyết minh tự động,
- thao tác đơn giản cho du khách.

Sản phẩm hiện tại đã đạt được phần lõi quan trọng của bài toán và có thể tiếp tục mở rộng ở các giai đoạn sau.
