# Danh sách xử lý các API Gateway

### Tổng quan:
- _**APIGateway**_: Đóng vai trò cổng vào duy nhất (entry point) cho client, định tuyến các yêu cầu HTTP đến các service tương ứng dựa trên URL.
	- Hoạt động:
		- Client gửi yêu cầu đến Gateway, ví dụ:
			```
			POST http://localhost:5000/api/users/register
			```
### 1. Xử lý cho Authenticate
