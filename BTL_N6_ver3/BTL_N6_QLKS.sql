create database BTL_N6_QLKS

use BTL_N6_QLKS
go

CREATE TABLE DangNhap (
    HoTen NVARCHAR(100) NOT NULL,
    TenTaiKhoan VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(255) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL
);


CREATE TABLE KhachHang (
    CCCD nvarchar(50) PRIMARY KEY,
    Ho_Ten NVARCHAR(100) NOT NULL,
    Ngay_Sinh DATE NOT NULL,
    Gioi_Tinh NVARCHAR(10) not null,
    So_Dien_Thoai VARCHAR(15) UNIQUE NOT NULL,
    Que_Quan NVARCHAR(255) NOT NULL,
    Trang_Thai NVARCHAR(50)
);

CREATE TABLE DatPhong (
    CCCD nvarchar(50) not null,
	Ma_Phong nvarchar(100) not null, 
	So_Nguoi int not null,
	Gia_Phong int not null,
	Ngay_Den nvarchar(50)
    
    FOREIGN KEY (CCCD) REFERENCES KhachHang(CCCD) ON DELETE CASCADE
);

CREATE TABLE DatPhongDonLe (
    CCCD nvarchar(50) not null,
	Ma_Phong nvarchar(100) not null, 
	So_Nguoi int not null,
	Gia_Phong int not null,
	Ngay_Den nvarchar(50),
	
    FOREIGN KEY (CCCD) REFERENCES KhachHang(CCCD) ON DELETE CASCADE
);

create table loaiPhong (
	Loai_Phong nvarchar(50) not null primary key,
	So_Nguoi_Toi_Da int not null,
	Gia_Phong_Theo_Ngay int not null
)

create table HoaDon (
	Ngay_Thanh_Toan nvarchar(50),
	CCCD nvarchar(50),
	Ho_Ten nvarchar(50),
	Ma_Phong nvarchar(50),
	Gia_Phong int,
	So_Ngay_O int,

	foreign key (CCCD) references KhachHang(CCCD) on delete cascade
)

insert into loaiPhong values 
(N'Phòng đơn',2,100000),
(N'Phòng đôi', 5, 200000),
(N'Phòng nhóm',10, 500000);

select * from DangNhap
select * from KhachHang
select * from loaiPhong
select * from DatPhong
select * from DatPhongDonLe
select * from HoaDon



INSERT INTO KhachHang (CCCD, Ho_Ten, Ngay_Sinh, Gioi_Tinh, So_Dien_Thoai, Que_Quan, Trang_Thai) VALUES
('10001', N'Lý Đình Sơn', '2005-08-30', N'Nam', '0912345678', N'Hà Nội', N'Chưa đặt phòng'),
('10002', N'Dương Nguyên Anh', '2005-08-31', N'Nữ', '0912346788', N'Nghệ An', N'Chưa đặt phòng'),
('10003', N'Đinh Phương Ly', '2005-12-27', N'Nữ', '0912345234', N'Hà Nội', N'Chưa đặt phòng'),
('10004', N'Nguyễn Văn An', '1995-05-10', N'Nam', '0910345678', N'Hà Nội', N'Chưa đặt phòng'),
('10005', N'Trần Thị Bích', '1998-08-20', N'Nữ', '0923456789', N'Hải Phòng', N'Chưa đặt phòng'),
('10006', N'Lê Văn Cường', '2000-11-15', N'Nam', '0934567890', N'Đà Nẵng', N'Chưa đặt phòng'),
('10007', N'Phạm Thị Dung', '1993-03-25', N'Nữ', '0945678901', N'Hồ Chí Minh', N'Chưa đặt phòng'),
('10008', N'Hoàng Văn Nam', '1997-07-12', N'Nam', '0956789012', N'Cần Thơ', N'Chưa đặt phòng'),
('10009', N'Đặng Thị Thủy', '1999-09-05', N'Nữ', '0967890123', N'Bắc Ninh', N'Chưa đặt phòng'),
('10010', N'Bùi Văn Thắng', '1992-02-18', N'Nam', '0978901234', N'Hà Tĩnh', N'Chưa đặt phòng'),
('10011', N'Vũ Thị Hương', '2001-06-30', N'Nữ', '0989012345', N'Nam Định', N'Chưa đặt phòng'),
('10012', N'Ngô Văn Minh', '1996-12-10', N'Nam', '0990123456', N'Nghệ An', N'Chưa đặt phòng')


create table tinhthanhVN (
	diadiem nvarchar(50)
)
INSERT INTO tinhthanhVN (diadiem) VALUES
(N'Hà Nội'),
(N'TP Hồ Chí Minh'),
(N'Đà Nẵng'),
(N'Hải Phòng'),
(N'Bắc Giang'),
(N'Bắc Ninh'),
(N'Điện Biên'),
(N'Hà Giang'),
(N'Hà Nam'),
(N'Hà Tĩnh'),
(N'Hải Dương'),
(N'Hòa Bình'),
(N'Hưng Yên'),
(N'Khánh Hòa'),
(N'Lai Châu'),
(N'Lạng Sơn'),
(N'Lào Cai'),
(N'Nam Định'),
(N'Nghệ An'),
(N'Ninh Bình'),
(N'Phú Thọ'),
(N'Quảng Bình'),
(N'Quảng Nam'),
(N'Quảng Ngãi'),
(N'Quảng Ninh'),
(N'Quảng Trị'),
(N'Sơn La'),
(N'Thái Bình'),
(N'Thái Nguyên'),
(N'Thanh Hóa'),
(N'Thừa Thiên Huế'),
(N'Tuyên Quang'),
(N'Vĩnh Phúc'),
(N'Yên Bái');