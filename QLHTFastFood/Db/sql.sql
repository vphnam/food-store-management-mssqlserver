USE master
GO
CREATE DATABASE QLFastFood
GO
USE QLFastFood
GO

CREATE TABLE SIZE
(
	Size_ID VARCHAR(2),
	TenSize NVARCHAR(3),
	CONSTRAINT PK_SIZE PRIMARY KEY(Size_ID)
)
CREATE TABLE DONVI
(
	DonVi_ID VARCHAR(4),
	TenDonVi NVARCHAR(20),
	CONSTRAINT PK_UNIT PRIMARY KEY(DonVi_ID)
)
CREATE TABLE FEEDBACK
(
	Feedback_ID VARCHAR(10),
	HoTen NVARCHAR(100),
	Email VARCHAR(100),
	SDT CHAR(15),
	NgayTao DATETIME,
	NoiDung NVARCHAR(MAX),
	CONSTRAINT PK_FEEDBACK PRIMARY KEY(Feedback_ID)
)
GO
CREATE TABLE NHACUNGCAP
(
	NCC_ID VARCHAR(5),
	TenNCC NVARCHAR(50),
	DiaChi NVARCHAR(200),
	CONSTRAINT PK_SUPPLIER PRIMARY KEY(NCC_ID)
)
GO
CREATE TABLE NGUYENLIEU
(
	NguyenLieu_ID VARCHAR(5),
	NCC_ID VARCHAR(5) NOT NULL,
	TenNguyenLieu NVARCHAR(100),
	DonVi_ID VARCHAR(4),
	SoLuongTon DECIMAL(18,2) CHECK(SoLuongTon >=0),
	MoTa NVARCHAR(200),
	CONSTRAINT PK_STOCK PRIMARY KEY(NguyenLieu_ID),
	CONSTRAINT FK_NCC_NGUYENLIEU FOREIGN KEY(NCC_ID) REFERENCES NHACUNGCAP(NCC_ID),
	CONSTRAINT FK_DONVI_NGUYENLIEU FOREIGN KEY(DonVi_ID) REFERENCES DONVI(DonVi_ID)
)
GO
CREATE TABLE VOUCHER
(
	Voucher_ID VARCHAR(10),
	TenVoucher NVARCHAR(100),
	PhanTramKhuyenMai FLOAT,
	GiamGiaToiDa DECIMAL(18,0),
	NgayBatDau DATE,
	NgayKetThuc DATE,
	MoTa NVARCHAR(250),
	CONSTRAINT PK_VOUCHER PRIMARY KEY(Voucher_ID),
)
GO
ALTER TABLE VOUCHER ADD CONSTRAINT CHECKDATE_VOUCHER CHECK(NgayBatDau < NgayKetThuc)
CREATE TABLE KHUYENMAI
(
	KhuyenMai_ID VARCHAR(5),
	TenKhuyenMai NVARCHAR(100),
	PhanTramKhuyenMai FLOAT CHECK(PhanTramKhuyenMai > 0 AND PhanTramKhuyenMai <= 1),
	NgayBatDau DATE,
	NgayKetThuc DATE,
	MoTa NVARCHAR(250),
	CONSTRAINT PK_KHUYENMAI PRIMARY KEY(KhuyenMai_ID)
)
GO
ALTER TABLE KHUYENMAI ADD CONSTRAINT CHECKDATE_KHUYENMAI CHECK(NgayBatDau < NgayKetThuc)

CREATE TABLE LOAI
(
	Loai_ID VARCHAR(5),
	TenLoai NVARCHAR(30),
	CONSTRAINT PK_LOAI PRIMARY KEY(Loai_ID) 
)
GO
CREATE TABLE MON
(
	Mon_ID VARCHAR(5),
	TenMon NVARCHAR(100),
	Loai_ID VARCHAR(5),
	Size_ID VARCHAR(2),
	DonVi_ID VARCHAR(4),
	AnhMon VARCHAR(100),
	DonGia DECIMAL(18,0) CHECK(DonGia >= 0),
	KhuyenMai_ID VARCHAR(5),
	MoTa NVARCHAR(200) NOT NULL,
	CONSTRAINT PK_MON PRIMARY KEY(Mon_ID),
	CONSTRAINT FK_KM_MON FOREIGN KEY(KhuyenMai_ID) REFERENCES KHUYENMAI(KhuyenMai_ID),
	CONSTRAINT FK_LOAI_MON FOREIGN KEY(Loai_ID) REFERENCES LOAI(Loai_ID),
	CONSTRAINT FK_SIZE_MON FOREIGN KEY(Size_ID) REFERENCES SIZE(Size_ID),
	CONSTRAINT FK_DONVI_MON FOREIGN KEY(DonVi_ID) REFERENCES DONVI(DonVi_ID)
)
GO

CREATE TABLE CONGTHUC
(
	Mon_ID VARCHAR(5),
	NguyenLieu_ID VARCHAR(5),
	SoLuongNLCan DECIMAL(18,2),
	CONSTRAINT PK_CONGTHUC PRIMARY KEY(Mon_ID,NguyenLieu_ID),
	CONSTRAINT FK_MON_CT FOREIGN KEY(Mon_ID) REFERENCES MON(Mon_ID),
	CONSTRAINT FK_NL_CT FOREIGN KEY(NguyenLieu_ID) REFERENCES NGUYENLIEU(NguyenLieu_ID)
)
GO
CREATE TABLE KHACHHANG
(
	KhachHang_ID VARCHAR(10),
	HoTen NVARCHAR(100),
	SDTKH CHAR(15) UNIQUE,
	Email VARCHAR(100) UNIQUE,
	UserName VARCHAR(100) UNIQUE,
	PassWord VARCHAR(100),
	DiaChi NVARCHAR(200),
	Status BIT DEFAULT(1),
	CONSTRAINT PK_KHACHHANG PRIMARY KEY (KhachHang_ID)
)
GO
CREATE TABLE QUYENTRUYCAP
(
	Quyen_ID VARCHAR(4),
	TenQuyen NVARCHAR(50),
	CONSTRAINT PK_PERMISSION PRIMARY KEY(Quyen_ID)
)
CREATE TABLE NHANVIEN
(
	NhanVien_ID VARCHAR(5),
	HoTenNV NVARCHAR(50),
	UserName VARCHAR(100) UNIQUE,
	PassWord VARCHAR(100),
	Quyen_ID VARCHAR(4),
	NgaySinh DATE,
	DiaChi NVARCHAR(MAX),
	SDT CHAR(15) UNIQUE,
	Email VARCHAR(100) UNIQUE,
	Status BIT DEFAULT(1),
	CONSTRAINT PK_NHANVIEN PRIMARY KEY(NhanVien_ID),
	CONSTRAINT FK_QUYEN_NV FOREIGN KEY(Quyen_ID) REFERENCES QUYENTRUYCAP(Quyen_ID)
)
GO
CREATE TABLE DONHANG
(
	DonHang_ID VARCHAR(10),
	KhachHang_ID VARCHAR(10),
	NhanVien_ID VARCHAR(5),
	Loai BIT,
	DaXacNhan BIT,
	DaThanhToan BIT,
	ThoiGianDat DATETIME,
	ThoiGianGiao DATETIME,
	Voucher_ID VARCHAR(10),
	CONSTRAINT PK_DONHANG PRIMARY KEY(DonHang_ID),
	CONSTRAINT FK_KHACHHANG_DH FOREIGN KEY(KhachHang_ID) REFERENCES KHACHHANG(KhachHang_ID),
	CONSTRAINT FK_NHANVIEN_DH FOREIGN KEY(NhanVien_ID) REFERENCES NHANVIEN(NhanVien_ID),
	CONSTRAINT FK_VOUCHER_DH FOREIGN KEY(Voucher_ID) REFERENCES VOUCHER(Voucher_ID)
)
GO
CREATE TABLE CTDONHANG
(
	DonHang_ID VARCHAR(10),
	Mon_ID VARCHAR(5),
	DonGiaMua DECIMAL(18,0),
	SoLuongMua INT CHECK (SoLuongMua >= 0),
	ThanhTien DECIMAL(18,0) CHECK (ThanhTien >= 0) DEFAULT(0),
	CONSTRAINT PK_CTDONHANGONL PRIMARY KEY(DonHang_ID, Mon_ID),
	CONSTRAINT FK_DONHANGONL_CTDONHANGONL FOREIGN KEY(DonHang_ID) REFERENCES DONHANG(DonHang_ID),
	CONSTRAINT FK_MON_CTDONHANGONL FOREIGN KEY(Mon_ID) REFERENCES MON(Mon_ID)
)
GO
CREATE TABLE CTDOANHTHUNGAY
(
	NgayGio DATETIME,
	DonHang_ID VARCHAR(10),
	Loai BIT,
	TongTienHoaDon DECIMAL(18,0),
	CONSTRAINT PK_CTDOANHTHUNGAY PRIMARY KEY(NgayGio, DonHang_ID),
	CONSTRAINT FK_CTDOANHTHUNGAY_DONHANG FOREIGN KEY(DonHang_ID) REFERENCES DONHANG(DonHang_ID)
)
GO

CREATE TRIGGER TAO_ID_CHO_DONVI ON DONVI INSTEAD OF INSERT AS
BEGIN
	DECLARE @UNIT_NAME NVARCHAR(20) = (SELECT TenDonVi FROM inserted)

	DECLARE @DEMDV INT = (SELECT COUNT(*) FROM DONVI)
	DECLARE @ID VARCHAR(4)
	SET @DEMDV += 1;
	DECLARE @LENGTHID VARCHAR(4)= 4 - ((SELECT LEN(@DEMDV)) + (SELECT LEN('DV')))
	IF(@LENGTHID = 1)
		SET @ID  = CONCAT('DV','0',@DEMDV)
	ELSE
	SET @ID  = CONCAT('DV',@DEMDV)

	DECLARE @DEMID INT = (SELECT COUNT(DonVi_ID) FROM DONVI WHERE DonVi_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMDV += 1;
		SET @LENGTHID = 4 - ((SELECT LEN(@DEMDV)) + (SELECT LEN('DV')))
		IF(@LENGTHID = 1)
			SET @ID  = CONCAT('DV','0',@DEMDV)
		ELSE
			SET @ID  = CONCAT('DV',@DEMDV)
		SET @DEMID = (SELECT COUNT(DonVi_ID) FROM DONVI WHERE DonVi_ID = @ID)
	END
	INSERT INTO DONVI(DonVi_ID,TenDonVi) VALUES(@ID,@UNIT_NAME)
END

CREATE TRIGGER TAO_ID_CHO_SIZE ON SIZE INSTEAD OF INSERT AS
BEGIN
	DECLARE @SIZE_NAME NVARCHAR(3) = (SELECT TenSize FROM inserted)

	DECLARE @DEMSIZE INT = (SELECT COUNT(*) FROM SIZE)
	DECLARE @ID VARCHAR(2)
	SET @DEMSIZE += 1;
		
	SET @ID  = CONCAT('S',@DEMSIZE)

	DECLARE @DEMID INT = (SELECT COUNT(Size_ID) FROM SIZE WHERE Size_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMSIZE += 1;

		SET @ID  = CONCAT('S',@DEMSIZE)
		SET @DEMID = (SELECT COUNT(Size_ID) FROM SIZE WHERE Size_ID = @ID)
	END
	INSERT INTO SIZE(Size_ID,TenSize) VALUES(@ID,@SIZE_NAME)
END

CREATE TRIGGER TAO_ID_CHO_FEEDBACK ON FEEDBACK INSTEAD OF INSERT AS
BEGIN
	DECLARE @HOTEN NVARCHAR(100) = (SELECT HoTen FROM inserted)
	DECLARE @EMAIL VARCHAR(100) = (SELECT Email FROM inserted)
	DECLARE @SDT CHAR(15) = (SELECT SDT FROM inserted)
	DECLARE @NGAYTAO DATETIME = (SELECT NgayTao FROM inserted)
	DECLARE @NOIDUNG NVARCHAR(MAX) = (SELECT NoiDung FROM inserted)

	DECLARE @DEMFB INT = (SELECT COUNT(*) FROM FEEDBACK)
	DECLARE @ID VARCHAR(10)
	SET @DEMFB += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(10) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 10 - ((SELECT LEN(@DEMFB)) + (SELECT LEN('FB')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('FB',@CHUOISOKHONG,@DEMFB)

	DECLARE @DEMID INT = (SELECT COUNT(Feedback_ID) FROM FEEDBACK WHERE Feedback_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMFB += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 10 - ((SELECT LEN(@DEMFB)) + (SELECT LEN('FB')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
		SET @ID  = CONCAT('FB',@CHUOISOKHONG,@DEMFB)
		SET @DEMID = (SELECT COUNT(Feedback_ID) FROM FEEDBACK WHERE Feedback_ID = @ID)
	END
	INSERT INTO FEEDBACK(Feedback_ID,HoTen,Email,SDT,NgayTao,NoiDung)
	VALUES(@ID,@HOTEN,@EMAIL,@SDT,@NGAYTAO,@NOIDUNG)
END


CREATE TRIGGER TAO_ID_CHO_NCC ON NHACUNGCAP INSTEAD OF INSERT AS
BEGIN
	DECLARE @SUPNAME NVARCHAR(100) = (SELECT TenNCC FROM inserted)
	DECLARE @DIACHI NVARCHAR(200) = (SELECT DiaChi FROM inserted)
	DECLARE @DEMSUP INT = (SELECT COUNT(*) FROM NHACUNGCAP)
	DECLARE @ID VARCHAR(5)
	SET @DEMSUP += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(5) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 5 - ((SELECT LEN(@DEMSUP)) + (SELECT LEN('NC')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('NC',@CHUOISOKHONG,@DEMSUP)

	DECLARE @DEMID INT = (SELECT COUNT(NCC_ID) FROM NHACUNGCAP WHERE NCC_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMSUP += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 5 - ((SELECT LEN(@DEMSUP)) + (SELECT LEN('NC')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
		SET @ID  = CONCAT('NC',@CHUOISOKHONG,@DEMSUP)
		SET @DEMID = (SELECT COUNT(NCC_ID) FROM NHACUNGCAP WHERE NCC_ID = @ID)
	END
	INSERT INTO NHACUNGCAP(NCC_ID,TenNCC,DiaChi) VALUES(@ID,@SUPNAME,@DIACHI)
END

CREATE TRIGGER TAO_ID_CHO_NGUYENLIEU ON NGUYENLIEU INSTEAD OF INSERT AS
BEGIN
	DECLARE @SUPID VARCHAR(5) = (SELECT NCC_ID FROM inserted)
	DECLARE @STOCKNAME NVARCHAR(100) = (SELECT TenNguyenLieu FROM inserted)
	DECLARE @UNIT VARCHAR(4) = (SELECT DonVi_ID FROM inserted)
	DECLARE @SLTON DECIMAL(18,2) = (SELECT SoLuongTon FROM inserted)
	DECLARE @MOTA NVARCHAR(200) = (SELECT MoTa FROM inserted)

	DECLARE @DEMSTOCK INT = (SELECT COUNT(*) FROM NGUYENLIEU)
	DECLARE @ID VARCHAR(5)
	SET @DEMSTOCK += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(5) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 5 - ((SELECT LEN(@DEMSTOCK)) + (SELECT LEN('NL')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('NL',@CHUOISOKHONG,@DEMSTOCK)

	DECLARE @DEMID INT = (SELECT COUNT(NguyenLieu_ID) FROM NGUYENLIEU WHERE NguyenLieu_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMSTOCK += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 5 - ((SELECT LEN(@DEMSTOCK)) + (SELECT LEN('NL')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
		SET @ID  = CONCAT('NL',@CHUOISOKHONG,@DEMSTOCK)
		SET @DEMID = (SELECT COUNT(NguyenLieu_ID) FROM NGUYENLIEU WHERE NguyenLieu_ID = @ID)
	END
	INSERT INTO NGUYENLIEU(NguyenLieu_ID,NCC_ID,TenNguyenLieu,DonVi_ID,SoLuongTon,MoTa) VALUES(@ID,@SUPID,@STOCKNAME,@UNIT,@SLTON,@MOTA)
END

CREATE TRIGGER TAO_ID_CHO_KHUYENMAI ON KHUYENMAI INSTEAD OF INSERT AS
BEGIN
	DECLARE @KM_NAME NVARCHAR(100) = (SELECT TenKhuyenMai FROM inserted)	
	DECLARE @PHANTRAMKM FLOAT = (SELECT PhanTramKhuyenMai FROM inserted)
	DECLARE @NGAYBD DATE = (SELECT NgayBatDau FROM inserted)
	DECLARE @NGAYKT DATE = (SELECT NgayKetThuc FROM inserted)
	DECLARE @MOTA NVARCHAR(200) = (SELECT MoTa FROM inserted)

	DECLARE @DEMKM INT = (SELECT COUNT(*) FROM KHUYENMAI)
	DECLARE @ID VARCHAR(5)
	SET @DEMKM += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(5) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 5 - ((SELECT LEN(@DEMKM)) + (SELECT LEN('KM')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('KM',@CHUOISOKHONG,@DEMKM)

	DECLARE @DEMID INT = (SELECT COUNT(KhuyenMai_ID) FROM KHUYENMAI WHERE KhuyenMai_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMKM += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 5 - ((SELECT LEN(@DEMKM)) + (SELECT LEN('KM')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
		SET @ID  = CONCAT('KM',@CHUOISOKHONG,@DEMKM)
		SET @DEMID = (SELECT COUNT(KhuyenMai_ID) FROM KHUYENMAI WHERE KhuyenMai_ID = @ID)
	END
	INSERT INTO KHUYENMAI(KhuyenMai_ID,TenKhuyenMai,PhanTramKhuyenMai,NgayBatDau,NgayKetThuc,MoTa) VALUES(@ID,@KM_NAME,@PHANTRAMKM,@NGAYBD,@NGAYKT,@MOTA)
END

CREATE TRIGGER TAO_ID_CHO_LOAI ON LOAI INSTEAD OF INSERT AS
BEGIN
	DECLARE @CATENAME NVARCHAR(30) = (SELECT TenLoai FROM inserted)	

	DECLARE @DEMCATE INT = (SELECT COUNT(*) FROM LOAI)
	DECLARE @ID VARCHAR(5)
	SET @DEMCATE += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(5) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 5 - ((SELECT LEN(@DEMCATE)) + (SELECT LEN('L')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('L',@CHUOISOKHONG,@DEMCATE)

	DECLARE @DEMID INT = (SELECT COUNT(Loai_ID) FROM LOAI WHERE Loai_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMCATE += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 5 - ((SELECT LEN(@DEMCATE)) + (SELECT LEN('L')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
		SET @ID  = CONCAT('L',@CHUOISOKHONG,@DEMCATE)
		SET @DEMID = (SELECT COUNT(Loai_ID) FROM LOAI WHERE Loai_ID = @ID)
	END
	INSERT INTO LOAI(Loai_ID,TenLoai) VALUES(@ID,@CATENAME)
END


CREATE TRIGGER TAO_ID_CHO_MON ON MON INSTEAD OF INSERT AS
BEGIN
	DECLARE @MONNAME NVARCHAR(100) = (SELECT TenMon FROM inserted)
	DECLARE @CATE VARCHAR(5) = (SELECT Loai_ID FROM inserted)
	DECLARE @SIZE VARCHAR(2) = (SELECT Size_ID FROM inserted)
	DECLARE @UNIT VARCHAR(4) = (SELECT DonVi_ID FROM inserted)
	DECLARE @IMG VARCHAR(100) = (SELECT AnhMon FROM inserted)
	DECLARE @DONGIA DECIMAL(18,0) = (SELECT DonGia FROM inserted)
	DECLARE @KM_ID VARCHAR(5) = (SELECT KhuyenMai_ID FROM inserted)
	DECLARE @MOTA NVARCHAR(200) = (SELECT MoTa FROM inserted)

	DECLARE @DEMMON INT = (SELECT COUNT(*) FROM MON)
	DECLARE @ID VARCHAR(5)
	SET @DEMMON += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(5) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 5 - ((SELECT LEN(@DEMMON)) + (SELECT LEN('M')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('M',@CHUOISOKHONG,@DEMMON)

	DECLARE @DEMID INT = (SELECT COUNT(Mon_ID) FROM MON WHERE Mon_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMMON += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 5 - ((SELECT LEN(@DEMMON)) + (SELECT LEN('M')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
		SET @ID  = CONCAT('M',@CHUOISOKHONG,@DEMMON)
		SET @DEMID = (SELECT COUNT(Mon_ID) FROM MON WHERE Mon_ID = @ID)
	END
	INSERT INTO MON(Mon_ID,TenMon,Loai_ID,Size_ID,DonVi_ID,AnhMon,DonGia,KhuyenMai_ID,MoTa) VALUES(@ID,@MONNAME,@CATE,@SIZE,@UNIT,@IMG,@DONGIA,@KM_ID,@MOTA)
END


CREATE TRIGGER TAO_ID_CHO_KHACHHANG ON KHACHHANG INSTEAD OF INSERT AS
BEGIN
	DECLARE @HOTEN NVARCHAR(100) = (SELECT HoTen FROM inserted)
	DECLARE @SDT CHAR(15) = (SELECT SDTKH FROM inserted)
	DECLARE @EMAIL VARCHAR(100) = (SELECT Email FROM inserted)
	DECLARE @USERNAME VARCHAR(100) = (SELECT UserName FROM inserted)
	DECLARE @PASSWORD VARCHAR(100) = (SELECT PassWord FROM inserted)
	DECLARE @DIACHI NVARCHAR(200) = (SELECT DiaChi FROM inserted)

	DECLARE @DEMKH INT = (SELECT COUNT(*) FROM KHACHHANG)
	DECLARE @ID VARCHAR(10)
	SET @DEMKH += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(10) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 10 - ((SELECT LEN(@DEMKH)) + (SELECT LEN('KH')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('KH',@CHUOISOKHONG,@DEMKH)

	DECLARE @DEMID INT = (SELECT COUNT(KhachHang_ID) FROM KHACHHANG WHERE KhachHang_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMKH += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 10 - ((SELECT LEN(@DEMKH)) + (SELECT LEN('KH')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
		SET @ID  = CONCAT('KH',@CHUOISOKHONG,@DEMKH)
		SET @DEMID = (SELECT COUNT(KhachHang_ID) FROM KHACHHANG WHERE KhachHang_ID = @ID)
	END
	INSERT INTO KHACHHANG(KhachHang_ID,HoTen,SDTKH,Email,UserName,PassWord,DiaChi,Status)
	VALUES(@ID,@HOTEN,@SDT,@EMAIL,@USERNAME,@PASSWORD,@DIACHI,1)
END


CREATE TRIGGER TAO_ID_CHO_QUYEN ON QUYENTRUYCAP INSTEAD OF INSERT AS
BEGIN
	DECLARE @PERMISSION_NAME NVARCHAR(50) = (SELECT TenQuyen FROM inserted)	

	DECLARE @DEMPER INT = (SELECT COUNT(*) FROM QUYENTRUYCAP)
	DECLARE @ID VARCHAR(4)
	SET @DEMPER += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(2) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 4 - ((SELECT LEN(@DEMPER)) + (SELECT LEN('Q')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('Q',@CHUOISOKHONG,@DEMPER)

	DECLARE @DEMID INT = (SELECT COUNT(Quyen_ID) FROM QUYENTRUYCAP WHERE Quyen_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMPER += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 4 - ((SELECT LEN(@DEMPER)) + (SELECT LEN('Q')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('Q',@CHUOISOKHONG,@DEMPER)
		SET @DEMID = (SELECT COUNT(Quyen_ID) FROM QUYENTRUYCAP WHERE Quyen_ID = @ID)
	END
	INSERT INTO QUYENTRUYCAP(Quyen_ID,TenQuyen) 
	VALUES(@ID, @PERMISSION_NAME)
END


CREATE TRIGGER TAO_ID_CHO_NHANVIEN ON NHANVIEN INSTEAD OF INSERT AS
BEGIN
	DECLARE @HOTEN NVARCHAR(50) = (SELECT HoTenNV FROM inserted)
	DECLARE @USERNAME VARCHAR(100) = (SELECT UserName FROM inserted)
	DECLARE @PASSWORD VARCHAR(100) = (SELECT PassWord FROM inserted)
	DECLARE @PER_ID VARCHAR(4) = (SELECT Quyen_ID FROM inserted)
	DECLARE @NGAYSINH DATE = (SELECT NgaySinh FROM inserted)
	DECLARE @DIACHI NVARCHAR(MAX) = (SELECT DiaChi FROM inserted)
	DECLARE @SDT CHAR(15) = (SELECT SDT FROM inserted)
	DECLARE @EMAIL VARCHAR(100) = (SELECT Email FROM inserted)

	DECLARE @DEMNV INT = (SELECT COUNT(*) FROM NHANVIEN)
	DECLARE @ID VARCHAR(5)
	SET @DEMNV += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(5) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 5 - ((SELECT LEN(@DEMNV)) + (SELECT LEN('NV')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('NV',@CHUOISOKHONG,@DEMNV)

	DECLARE @DEMID INT = (SELECT COUNT(NhanVien_ID) FROM NHANVIEN WHERE NhanVien_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMNV += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 10 - ((SELECT LEN(@DEMNV)) + (SELECT LEN('NV')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
		SET @ID  = CONCAT('NV',@CHUOISOKHONG,@DEMNV)
		SET @DEMID = (SELECT COUNT(NhanVien_ID) FROM NHANVIEN WHERE NhanVien_ID = @ID)
	END
	INSERT INTO NHANVIEN(NhanVien_ID,HoTenNV,UserName,PassWord,Quyen_ID,NgaySinh,DiaChi,SDT,Email,Status)
	VALUES(@ID,@HOTEN,@USERNAME,@PASSWORD,@PER_ID,@NGAYSINH,@DIACHI,@SDT,@EMAIL,1)
END


CREATE TRIGGER TAO_ID_CHO_DONHANG ON DONHANG INSTEAD OF INSERT AS
BEGIN
	DECLARE @KHACHHANG_ID VARCHAR(10) = (SELECT KhachHang_ID FROM inserted)
	DECLARE @NHANVIEN_ID VARCHAR(10) = (SELECT NhanVien_ID FROM inserted)
	DECLARE @LOAI BIT = (SELECT Loai FROM inserted)
	DECLARE @DAXACNHAN BIT = (SELECT DaXacNhan FROM inserted)
	DECLARE @DATHANHTOAN BIT = (SELECT DaThanhToan FROM inserted)
	DECLARE @TIMEDAT DATETIME = (SELECT ThoiGianDat FROM inserted)
	DECLARE @TIMEGIAO DATETIME = (SELECT ThoiGianGiao FROM inserted)
	DECLARE @VOUCHER_ID VARCHAR(10) = (SELECT Voucher_ID FROM inserted)

	DECLARE @DEMDH INT = (SELECT COUNT(*) FROM DONHANG)
	DECLARE @ID VARCHAR(10)
	SET @DEMDH += 1;
	
	DECLARE @CHUOISOKHONG VARCHAR(10) = '0'
	DECLARE @I INT = 0
	DECLARE @LENGTHID INT = 10 - ((SELECT LEN(@DEMDH)) + (SELECT LEN('DH')))
	WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END
	SET @ID  = CONCAT('DH',@CHUOISOKHONG,@DEMDH)
	DECLARE @DEMID INT = (SELECT COUNT(DonHang_ID) FROM DONHANG WHERE DonHang_ID = @ID)

	WHILE(@DEMID <> 0)
	BEGIN
		SET @DEMDH += 1;

		SET @CHUOISOKHONG = '0'
		SET @I = 0
		SET @LENGTHID = 10 - ((SELECT LEN(@DEMDH)) + (SELECT LEN('DH')))
		WHILE (@I < @LENGTHID - 1)
		BEGIN
			SET @CHUOISOKHONG = @CHUOISOKHONG + '0'
			SET @I += 1;
		END 
		SET @ID  = CONCAT('DH',@CHUOISOKHONG,@DEMDH)
		SET @DEMID = (SELECT COUNT(DonHang_ID) FROM DONHANG WHERE DonHang_ID = @ID)
	END
	INSERT INTO DONHANG(DonHang_ID,KhachHang_ID,NhanVien_ID,Loai,DaXacNhan,DaThanhToan,ThoiGianDat,ThoiGianGiao,Voucher_ID)
	VALUES(@ID,@KHACHHANG_ID,@NHANVIEN_ID,@LOAI,@DAXACNHAN,@DATHANHTOAN,@TIMEDAT,@TIMEGIAO,@VOUCHER_ID)
END

CREATE TRIGGER TRU_KHO_VA_THEM_DOANH_THU ON DONHANG FOR UPDATE AS
BEGIN
	DECLARE @DAXACNHAN BIT = (SELECT DaXacNhan FROM inserted)
	DECLARE @DATHANHTOAN BIT = (SELECT DaThanhToan FROM inserted)
	IF(@DAXACNHAN = 1 AND @DATHANHTOAN = 0)
	BEGIN
		BEGIN TRY
		
			DECLARE @DONHANG_ID VARCHAR(10) = (SELECT DonHang_ID FROM inserted)
			DECLARE @TABLEMONCANTRU TABLE (Mon_ID VARCHAR(5), SoLuongMua INT)
		
			INSERT INTO @TABLEMONCANTRU SELECT Mon_ID,SoLuongMua FROM CTDONHANG WHERE DonHang_ID = @DONHANG_ID
			DECLARE @BIEN VARCHAR(5) = (SELECT TOP(1) Mon_ID FROM @TABLEMONCANTRU)
			DECLARE @SL DECIMAL(18,2) = (SELECT TOP(1) SoLuongMua FROM @TABLEMONCANTRU)
			DECLARE @COUNT INT= (SELECT COUNT(*) FROM @TABLEMONCANTRU)
			DECLARE @TABLENGUYENLIEU TABLE(NguyenLieu_ID VARCHAR(5), SoLuongTru DECIMAL(18,2))	
			WHILE(@COUNT <> 0)
			BEGIN
				INSERT INTO @TABLENGUYENLIEU SELECT NguyenLieu_ID, SoLuongNLCan * @SL FROM CONGTHUC WHERE Mon_ID = @BIEN
				DELETE FROM @TABLEMONCANTRU WHERE Mon_ID = @BIEN
				SET @BIEN = (SELECT TOP(1) Mon_ID FROM @TABLEMONCANTRU)
				SET @SL= (SELECT TOP(1) SoLuongMua FROM @TABLEMONCANTRU)
				SET @COUNT = (SELECT COUNT(*) FROM @TABLEMONCANTRU)
			END
			SET @COUNT = (SELECT COUNT(*) FROM @TABLENGUYENLIEU)
			SET @BIEN = (SELECT TOP(1) NguyenLieu_ID FROM @TABLENGUYENLIEU)
			SET @SL= (SELECT TOP(1) SoLuongTru FROM @TABLENGUYENLIEU)
			WHILE(@COUNT <> 0)
			BEGIN
				UPDATE NGUYENLIEU SET SoLuongTon = SoLuongTon - @SL WHERE NguyenLieu_ID = @BIEN
				DELETE FROM @TABLENGUYENLIEU WHERE NguyenLieu_ID = @BIEN
				SET @BIEN = (SELECT TOP(1) NguyenLieu_ID FROM @TABLENGUYENLIEU)
				SET @SL= (SELECT TOP(1) SoLuongTru FROM @TABLENGUYENLIEU)
				SET @COUNT = (SELECT COUNT(*) FROM @TABLENGUYENLIEU)
			END
		END TRY
		BEGIN CATCH
			ROLLBACK
			RAISERROR('H???t h??ng',16,0)
		END CATCH
	END
	IF(@DAXACNHAN = 1 AND @DATHANHTOAN = 1)
	BEGIN
		DECLARE @MADH VARCHAR(10) = (SELECT DonHang_ID FROM inserted)
		DECLARE @NGAYGIO DATETIME = (SELECT ThoiGianGiao FROM DONHANG WHERE DonHang_ID = @MADH)
		DECLARE @LOAI BIT = (SELECT Loai FROM DONHANG)
		DECLARE @TONGTIEN DECIMAL(18,0) = (SELECT SUM(ThanhTien) FROM CTDONHANG WHERE DonHang_ID = @MADH)
		INSERT INTO CTDOANHTHUNGAY(NgayGio,DonHang_ID,Loai,TongTienHoaDon) VALUES(@NGAYGIO,@MADH,@LOAI,@TONGTIEN)
	END
END

INSERT INTO SIZE(TenSize)
VALUES('S')
INSERT INTO SIZE(TenSize)
VALUES('M')
INSERT INTO SIZE(TenSize)
VALUES('L')
SELECT * FROM SIZE

INSERT INTO DONVI(TenDonVi)
VALUES(N'Ph???n')
INSERT INTO DONVI(TenDonVi)
VALUES(N'C??i')
INSERT INTO DONVI(TenDonVi)
VALUES('Gram')
INSERT INTO DONVI(TenDonVi)
VALUES('Kg')
SELECT * FROM DONVI

INSERT INTO FEEDBACK(HoTen,Email,SDT,NgayTao,NoiDung)
VALUES(N'Th??i H??ng','hung123@gmail.com','0912456741',GETDATE(),N'????? ??n qu?? xu???t cmn s???c')
SELECT * FROM FEEDBACK

INSERT INTO NHACUNGCAP(TenNCC,DiaChi)
VALUES(N'VietGab - Nh?? cung c???p rau',N'123 Tr?????ng Chinh')
INSERT INTO NHACUNGCAP(TenNCC,DiaChi)
VALUES(N'Mr Meat - Nh?? cung c???p th???t',N'11 L?? Th?????ng Ki???t')
INSERT INTO NHACUNGCAP(TenNCC,DiaChi)
VALUES(N'Perfect Bread - Nh?? cung c???p b??nh m??',N'66 L?? Th??i T???')
SELECT * FROM NHACUNGCAP

INSERT INTO NGUYENLIEU(NCC_ID,TenNguyenLieu,DonVi_ID,SoLuongTon,MoTa)
VALUES('NC002',N'????i g??','DV04',10.5,N'????i g??')
INSERT INTO NGUYENLIEU(NCC_ID,TenNguyenLieu,DonVi_ID,SoLuongTon,MoTa)
VALUES('NC002',N'C??nh g??','DV04',100,N'????i g??')
INSERT INTO NGUYENLIEU(NCC_ID,TenNguyenLieu,DonVi_ID,SoLuongTon,MoTa)
VALUES('NC002',N'Th???t B??','DV04',10,N'Th???t b??')
INSERT INTO NGUYENLIEU(NCC_ID,TenNguyenLieu,DonVi_ID,SoLuongTon,MoTa)
VALUES('NC003',N'Burger','DV02',100,N'Burger')
INSERT INTO NGUYENLIEU(NCC_ID,TenNguyenLieu,DonVi_ID,SoLuongTon,MoTa)
VALUES('NC001',N'X?? l??ch','DV04',12.8,N'Rau x?? l??ch')
INSERT INTO NGUYENLIEU(NCC_ID,TenNguyenLieu,DonVi_ID,SoLuongTon,MoTa)
VALUES('NC001',N'Khoai t??y','DV04',12.8,N'Khoai t??y')
SELECT * FROM NGUYENLIEU

INSERT INTO VOUCHER(Voucher_ID,TenVoucher,PhanTramKhuyenMai,GiamGiaToiDa,NgayBatDau,NgayKetThuc,MoTa)
VALUES('T6VUIVE',N'Khuy???n m??i 10%',0.1,50000,'2021-6-1','2021-6-30','Khuy???n m??i ??p d???ng cho th??ng 6')
SELECT * FROM VOUCHER

INSERT INTO KHUYENMAI(TenKhuyenMai,PhanTramKhuyenMai,NgayBatDau,NgayKetThuc,MoTa)
VALUES(N'Gi???m gi?? 20% cho m??n',0.2,'2021-6-1','2021-6-30',N'Gi???m gi?? 20% cho c??c m??n v??o th??ng 6')
SELECT * FROM KHUYENMAI

INSERT INTO LOAI(TenLoai)
VALUES(N'UPSIZE')
INSERT INTO LOAI(TenLoai)
VALUES(N'COMBO')
INSERT INTO LOAI(TenLoai)
VALUES(N'M??N L???')
INSERT INTO LOAI(TenLoai)
VALUES(N'N?????C GI???I KH??T')
INSERT INTO LOAI(TenLoai)
VALUES(N'????? ??N PH???')
SELECT * FROM LOAI

INSERT INTO MON(TenMon,Loai_ID,Size_ID,DonVi_ID,AnhMon,DonGia,KhuyenMai_ID,MoTa)
VALUES(N'Combo 3 ????i g?? r??n + Hamburger','L0002','S2','DV02','~/Images/add.png',100000,'KM001',N'G???m 1 hamburger v?? 3 ????i g?? r??n')
INSERT INTO MON(TenMon,Loai_ID,Size_ID,DonVi_ID,AnhMon,DonGia,KhuyenMai_ID,MoTa)
VALUES(N'Khoai t??y chi??n 200g','L0002','S2','DV03','~/Images/add.png',20000,null,N'G???m 1 hamburger v?? 3 ????i g?? r??n')
SELECT * FROM MON

INSERT INTO CONGTHUC(Mon_ID,NguyenLieu_ID,SoLuongNLCan)
VALUES('M0001','NL001',0.6)
INSERT INTO CONGTHUC(Mon_ID,NguyenLieu_ID,SoLuongNLCan)
VALUES('M0001','NL003',0.3)
INSERT INTO CONGTHUC(Mon_ID,NguyenLieu_ID,SoLuongNLCan)
VALUES('M0001','NL005',0.1)
INSERT INTO CONGTHUC(Mon_ID,NguyenLieu_ID,SoLuongNLCan)
VALUES('M0002','NL006',0.2)
SELECT * FROM CONGTHUC

INSERT INTO KHACHHANG(HoTen,SDTKH,Email,UserName,PassWord,DiaChi)
VALUES(N'Hu???nh Tr???n Th??nh','0915123546','thanhmatau@gmail.com','thanhmatau','123456',N'1 Th??nh Th??i')
SELECT * FROM KHACHHANG

INSERT INTO QUYENTRUYCAP(TenQuyen)
VALUES(N'ADMIN')
INSERT INTO QUYENTRUYCAP(TenQuyen)
VALUES(N'K??? to??n')
INSERT INTO QUYENTRUYCAP(TenQuyen)
VALUES(N'Thu ng??n')
INSERT INTO QUYENTRUYCAP(TenQuyen)
VALUES(N'Nh??n vi??n')
SELECT * FROM QUYENTRUYCAP

INSERT INTO NHANVIEN(HoTenNV,UserName,PassWord,Quyen_ID,NgaySinh,DiaChi,SDT,Email)
VALUES(N'Nam V??','namadmin','161120','Q001','2000-11-16',N'20 ???????ng s??? 5','0917265477','hoainam11134@gmail.com')
INSERT INTO NHANVIEN(HoTenNV,UserName,PassWord,Quyen_ID,NgaySinh,DiaChi,SDT,Email)
VALUES(N'Sang Kh???','asangketoan','123456','Q002','2000-6-1',N'20/12 T??n ?????n','0912485456','sangkhomaiyeuminhem@gmail.com')
SELECT * FROM NHANVIEN

INSERT INTO DONHANG(KhachHang_ID,NhanVien_ID,Loai,DaXacNhan,DaThanhToan,ThoiGianDat,ThoiGianGiao,Voucher_ID)
VALUES('KH00000001','NV001',0,0,0,GETDATE(),'2021-5-28 20:00:00',null)
SELECT * FROM DONHANG

INSERT INTO CTDONHANG(DonHang_ID,Mon_ID,DonGiaMua,SoLuongMua,ThanhTien)
VALUES('DH00000001','M0001',80000,2,160000)
INSERT INTO CTDONHANG(DonHang_ID,Mon_ID,DonGiaMua,SoLuongMua,ThanhTien)
VALUES('DH00000001','M0002',20000,1,20000)
SELECT * FROM CTDONHANG 

UPDATE DONHANG SET DaXacNhan = 1 WHERE DonHang_ID = 'DH00000001'
10.5
100
10
100
12.8
12.8
SELECT * FROM NGUYENLIEU

UPDATE DONHANG SET DaThanhToan = 1 WHERE DonHang_ID = 'DH00000001'
SELECT * FROM CTDOANHTHUNGAY
