# Danh sách xử lý các API Gateway

### Tổng quan:

- _**APIGateway**_: Đóng vai trò cổng vào duy nhất (entry point) cho client, định tuyến các yêu cầu HTTP đến các service
  tương ứng dựa trên URL.
    - Hoạt động:
        - Client gửi yêu cầu đến Gateway, ví dụ:
      ```bash
      POST http://localhost:5000/api/users/register
      ```
        - APIGateway kiểm tra cấu hình định tuyến và chuyển tiếp yêu cầu đến UserService:
      ```bash
      POST http://localhost:5001/api/users/register
      ```
        - Nếu client cần truy cập EmailService, APIGateway chuyển tiếp đến EmailService dựa trên cấu hình.
            - **Không xử lý logic**: APIGateway chỉ định tuyến mà không thực hiện xử lý dữ liệu hay logic kinh doanh.
- _**UserService**_: Quản lý các tác vụ liên quan đến người dùng như đăng ký, đăng nhập, và xác thực,...
    - Hoạt động (VD đăng kí tài khoản):
        1. Khi nhận được yêu cầu đăng ký, UserService:
            - Lưu thông tin người dùng vào cơ sở dữ liệu.
            - Gửi sự kiện qua Redis Pub/Sub để thông báo cho NotificationService.
            - Trả về phản hồi thành công cho client qua APIGateway.
            - Ví dụ luồng đăng ký người dùng:
              ```json
              POST http://localhost:5000/api/users/register
              {
                  "email": "abc@xyz.com",
                  "password": "password123"
              }
              ```
                - APIGateway định tuyến yêu cầu đến UserService:
                - UserService:
                    - Lưu thông tin người dùng vào database.
                    - Publish một sự kiện lên Redis với nội dung:
                    ```json
                    {
                       "Email": "abc@xyz.com"
                    }
                    ```
                    - Trả về phản hồi:
                    ```json
                    {
                       "message": "User registered successfully"
                    }
                    ```
- _**NotificationService**_: Xử lý gửi email hoặc thông báo liên quan
  - Hoạt động:
    - Lắng nghe sự kiện Redis trên channel <code>UserRegistered</code>.
    - Khi có sự kiện, NotificationService:
      1. Parse dữ liệu từ sự kiện.
      2. Gửi email hoặc thông báo tới mục tiêu được chỉ định.
    - Ví dụ xử lý sự kiện Redis:
      - Sự kiện Redis: Khi UserService publish một sự kiện:
      ```json
        {
           "Email": "abc@xyz.com"
        }
      ```
      - Gửi email: NotificationService sử dụng IEmailService để gửi email đến người dùng:
      ```text
        To: user@example.com
        Subject: Welcome!
        Body: Thank you for registering with our service.
      ```
### 1. Kiến trúc tổng quan cho Authenticate
