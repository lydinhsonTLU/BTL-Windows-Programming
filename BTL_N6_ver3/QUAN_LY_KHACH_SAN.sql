CREATE DATABASE QuanLyKhachSan;
GO

USE QuanLyKhachSan;
GO

-- B?ng ??ng Nh?p
CREATE TABLE DangNhap (
    HoTen NVARCHAR(100) NOT NULL,
    TenTaiKhoan VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(255) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL
);

-- B?ng Thông Tin Khách Hàng
CREATE TABLE KhachHang (
    CCCD CHAR(12) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'N?', N'Khác')) NOT NULL,
    SoDienThoai VARCHAR(15) UNIQUE NOT NULL,
    QueQuan NVARCHAR(255) NOT NULL,
    SoPhong INT NULL,
    TrangThai NVARCHAR(50) NULL
);

-- B?ng Thông Tin ??t Phòng (1 khách hàng có th? ??t nhi?u phòng)
CREATE TABLE DatPhong (
    MaDatPhong INT IDENTITY(1,1) PRIMARY KEY,
    CCCD CHAR(12) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    NgayDat DATE NOT NULL,
    NgayTra DATE NOT NULL,
    LoaiPhong NVARCHAR(50) NOT NULL,
    FOREIGN KEY (CCCD) REFERENCES KhachHang(CCCD) ON DELETE CASCADE
);

select *from DangNhap;
select *from KhachHang;