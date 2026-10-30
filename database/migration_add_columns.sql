USE QuanLySuKienVaBanVe;
GO

-- 1. Thêm cột LoaiSoDo vào bảng SuatDien
IF COL_LENGTH('SuatDien', 'LoaiSoDo') IS NULL
BEGIN
    ALTER TABLE SuatDien ADD LoaiSoDo INT NOT NULL CONSTRAINT DF_SuatDien_LoaiSoDo DEFAULT 1;
END
GO
UPDATE SuatDien SET LoaiSoDo = 1 WHERE LoaiSoDo IS NULL;
GO

-- 2. Thêm cột ChoPhepBanVe vào bảng KhuVuc
IF COL_LENGTH('KhuVuc', 'ChoPhepBanVe') IS NULL
BEGIN
    ALTER TABLE KhuVuc ADD ChoPhepBanVe BIT NOT NULL CONSTRAINT DF_KhuVuc_ChoPhepBanVe DEFAULT 1;
END
GO

-- 3. Thêm ràng buộc sức chứa > 0 (tránh nhập số âm)
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_KhuVuc_TongSoGhe')
BEGIN
    UPDATE KhuVuc SET TongSoGhe = 1 WHERE TongSoGhe <= 0;
    ALTER TABLE KhuVuc WITH CHECK ADD CONSTRAINT CK_KhuVuc_TongSoGhe CHECK (TongSoGhe > 0 AND TongSoGhe <= 10000);
END
GO