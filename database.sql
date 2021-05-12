#create database testing;
use testing;


######### CREATE TABLE

### CHAT LIEU

create table chatlieu(
	id_cl varchar(50) primary key,
    ten_cl varchar(50)
);

insert into chatlieu value ('ct01','cotton');
insert into chatlieu values ('ct02','cotton pha');
insert into chatlieu values ('kk01',' kiki');
insert into chatlieu values ('j01',' jeans');
insert into chatlieu values ('k03',' kate');
insert into chatlieu values ('n01',' ni');
insert into chatlieu values ('l01',' len');
insert into chatlieu values ('l02',' lanh');
insert into chatlieu values ('t01',' tho');
insert into chatlieu values ('v01',' voan');
insert into chatlieu values ('r01',' ren');
insert into chatlieu values ('l03',' lua tu nhien');
insert into chatlieu values ('cf01',' chilffon');
insert into chatlieu values ('tm01',' tuyet mua');
insert into chatlieu values ('nl',' ni long');
insert into chatlieu values ('da01',' da ca sau');
insert into chatlieu values ('da02',' da bo');
insert into chatlieu values ('da03',' da tran');
insert into chatlieu values ('cl00',' chat lieu khac');

-- select * from chatlieu where id_cl = '';
-- delete from chatlieu where id_cl = '';
-- update chatlieu set ten_cl = 'da gau' where id_cl = 'da03';
-- Insert into chatlieu values ('l04','latex');


create table khach(
	id_kh int primary key auto_increment,
    tenkhach varchar(50),
    diachi varchar(100),
    sdt varchar(15)
);
-- select * from khach;
-- drop table khach;

insert into khach (tenkhach, diachi, sdt) value('Truong Vo Ky','Mong Nguyen','0919909019');
insert into khach (tenkhach, diachi, sdt) value('Duong Qua','Nam Tong','0919900000');
insert into khach (tenkhach, diachi, sdt) value('Quach Tinh','Tong','0919111119');
insert into khach (tenkhach, diachi, sdt) value('Lenh Ho Xung','Minh','0949094909');
insert into khach (tenkhach, diachi, sdt) value('Doan Du','Dai Ly','0998877665');
insert into khach (tenkhach, diachi, sdt) value('Vi Tieu Bao','Thanh','0944445555');
insert into khach (tenkhach, diachi, sdt) value('Tran Gia Lac','Thanh','0912345678');
insert into khach (tenkhach, diachi, sdt) value('Ho Phi','Thanh','0969216754');

-- update khach set tenkhach = 'Bao Thanh Thien', diachi = 'Bac Tong', sdt  = 053999371062;

### HANG HOA

create table hanghoa(
	id_hang varchar(10) primary key,
    tenhang varchar(50),
    id_cl varchar(10),
    soluong float,
    giamua float,
    giaban float,
    ghichu varchar(200),
    foreign key(id_cl) references chatlieu(id_cl)
);
-- drop table hanghoa;
# select * from hanghoa where id_hang = "Q01"

# 	select * from 
insert into hanghoa value('Q01','quan kaki','kk01',101,105000,150000,'quan dai');
insert into hanghoa value('Q02','quan jean','j01',150,130000,180000,'quan dai');
insert into hanghoa value('Q03','quan kaki','kk01',70,70000,90000,'quan cut');
insert into hanghoa value('A01','Ao somi','k03',80,110000,170000,'ao tay dai');
insert into hanghoa value('A02','Ao lua','l03',50,21000,240000,'ao tay dai');
insert into hanghoa value('A03','Ao ren','r01',30,70000,85000,'ao tay dai');
insert into hanghoa value('A04','Ao thun','ct01',45,95000,110000,'ao tay dai');
insert into hanghoa value('A05','Ao thun','ct01',88,75000,90000,'ao tay ngan');
insert into hanghoa value('A06','Ao thun','k03',10,85000,100000,'ao tay lo');
insert into hanghoa value('TL01','day that lung','da01',25,290000,340000,'da ca sau');
insert into hanghoa value('TL02','day that lung','da01',20,300000,355000,'da bo');
insert into hanghoa value('TL03','that lung','cl00',100,65000,80000,'kim loai');
insert into hanghoa value('Tui01','tui deo cheo','da01',17,160000,180000,'tui da');
insert into hanghoa value('Tui02','tui deo cheo','t01',46,50000,650000,'tui vai tho');
insert into hanghoa value('vi01','bop/vi','da01',50,300000,330000,'vi da');
insert into hanghoa value('vi02','bop/vi','cl00',190,100000,120000,'chat lieu khac');

-- Delete from hanghoa where id_hang = '';

-- select * from hanghoa;

/*
-- ---------------------------------- nhan vien
create table nhanvien(
	id_nv varchar(10) primary key,
    ten_nv varchar(50),
    gioitinh int,
    diachi varchar(100),
    sdt varchar(15),
    ngaysinh date
);
insert into nhanvien value('nv001','lai trung nghia',1,'can tho','0839596813','1999-7-15');
insert into nhanvien value('nv002','tran chan bao',1,'can tho','0839596813','2000-11-20');
insert into nhanvien value('nv003','tram tien anh',0,'can tho','0839596813','1998-7-15');
insert into nhanvien value('nv004','Ngoc Bien',0,'can tho','0839596813','1999-7-15');
insert into nhanvien value('nv005','Ba Toan',0,'can tho','0839596813','1999-7-15');
insert into nhanvien value('nv006','Buoi Phan',0,'can tho','0839596813','1999-7-15');
select * from nhanvien;
*/

create table RegisterUser(
	id 			int primary key auto_increment,
    lastName 	varchar(50) default 'Users Lastname',
    userName 	varchar(50) not null,
    password 	varchar(50) not null,
    email	 	varchar(50) default 'Users Email',
    type		int not null default 0, # 0 is staff , 1 is admin
    userDescription nvarchar(200) default 'This is your description'
);


-- drop table RegisterUser;

-- select *from registeruser;

-- Select *from RegisterUser where userName = 'Mai';

-- insert into RegisterUser(lastName,username,passWord,email) values ('Trung Nghia','Nghia','2','Nghia2@gmail.com');

-- update RegisterUser set type = 1 where username = 'Nghia';

-- Delete from RegisterUser where username = 'Nghia';

-- select * from RegisterUser;

### HOA DON BAN

create table HDban(
	id_hd varchar(50) primary key,
    id_nv int auto_increment,
    ngayban nvarchar(50),
    id_kh int,
    tongtien float,
    foreign key (id_nv) references RegisterUser(id),
    foreign key (id_kh) references khach(id_kh)
);

-- drop table HDban;
insert into HDban values('hd001','1','2019-11-15','1',340000);
insert into HDban values('hd002','8','2019-11-15','kh005',280000);
insert into HDban values('hd003','9','2019-12-15','kh001',120000);
insert into HDban values('hd004','7','2019-11-16','kh007',340000);
insert into HDban values('hd005','8','2019-11-17','kh004',280000);
insert into HDban values('hd006','9','2019-12-18','kh003',120000);

-- Delete from HDban where id_hd = 'HD20210508';
# select id_hd from HDban;
# select * from HDban where id_hd = "hd001";
-- select * from HDban;
#select * from khach;
-- select count(*) as 'Tong so hoa don' from HDban;
# select * from khach where id_kh = 1;
#select tenkhach, diachi, sdt from khach where id_kh = 1;

### CHI TIET HOA DON BAN

create table chitietHDban(
	id_hd varchar(50),
    id_hang varchar(10),
    soluong float,
    dongia float,
    giamgia float,
    thanhtien float,
    foreign key (id_hd) references HDban(id_hd),
    foreign key (id_hang) references hanghoa(id_hang)
);

insert into chitietHDban value('hd001','TL01',1,340000,0,340000);
insert into chitietHDban value('hd002','tui01',1,180000,0,180000);
insert into chitietHDban value('hd002','A06',1,100000,0,100000);
insert into chitietHDban value('hd003','vi02',1,120000,0,120000);
insert into chitietHDban value('hd004','A02',1,240000,0,240000);
insert into chitietHDban value('hd004','A04',1,110000,10000,100000);
insert into chitietHDban value('hd005','tui01',1,180000,0,180000);
insert into chitietHDban value('hd005','A06',1,100000,0,100000);
insert into chitietHDban value('hd006','vi02',1,120000,0,120000);

-- Delete from chitietHDban where id_hd = 'hd002';

-- select * from chitiethdban;
-- select * from HDban;
-- select * from hanghoa;

# drop table chitiethdban;

######### DELIMITER

delimiter $$
create procedure ShowUser()
begin 
	select id as 'ID Nhan vien', lastName as 'Ten hien thi', userName as 'Ten dang nhap',
	password as 'Mat khau', email as 'Email', type as 'Loai tai khoan', userDescription as 'Mo ta ban than' from RegisterUser;
end $$
DELIMITER 

# drop procedure ShowUser;

delimiter $$
create procedure UpdateUserInformation(p_id int, p_lastname varchar(50), p_password varchar(50), p_email varchar(50), p_type int, p_description nvarchar(200))
begin
	update RegisterUser set lastname = p_lastname , password = p_password , email = p_email , type = p_type, userDescription = p_description where id = p_id;
end $$
delimiter

delimiter $$
create procedure SP_DeleteUser(p_id int)
begin
	delete from registerUser where id = p_id;
end $$
delimiter


# drop procedure UpdateUserInformation;

select * from registerUser;

call SP_DeleteUser(5);

call ShowUser();

call AddUser('ltnghia','nghia','1','ltnghia@gmail.com');

call UpdateUserInformation(1, 'ltnghia', 1, 'ltnghia@gmail.com',0,"Day la mo ta ban than cua toi");

call UpdateUserInformation( 2 , 'thinhtran' , 1 , 'thinh@gmail.com' , 1 , 'bblahblah' );

#Update RegisterUser set lastName = @lastname , email = @email , type = @type where username = @username ";

delimiter $$
create procedure ShowCustomer()
begin 
	select id_kh as 'ID khach hang', tenkhach as 'Ten khach hang', diachi as 'Dia chi', sdt as 'So dien thoai' from khach;
end $$
DELIMITER 

delimiter $$
create procedure ShowProduce()
begin 
	select id_hang as 'ID Hang', tenhang as 'Ten mat hang', id_cl as 'ID Chat lieu', 
	soluong as 'So luong', giamua as 'Gia nhap hang', giaban as 'Gia ban ra', ghichu as 'Mo ta mat hang' from hanghoa;
end $$
DELIMITER 

delimiter $$
create procedure ShowMat()
begin 
	select id_cl as 'Ma chat lieu',  ten_cl as 'Ten chat lieu' from chatlieu;
end $$
DELIMITER 

call SP_InvoiceInfo('hd001')

select * from khach
call ShowMat();
call ShowCustomer();
call ShowProduce();
call ShowUser();

delimiter $$
create procedure SP_ShowMaterial(p_id_chatlieu varchar(50))
begin 
	select ten_cl as 'ten chat lieu' from chatlieu where id_cl = p_id_chatlieu;
end $$
DELIMITER 
#select ten_cl from chatlieu where id_cl = 'ct01';
#call SP_ShowMaterial('ct01');

#select *from hanghoa;

delimiter $$
create procedure SP_UpdateProduct(p_tenHang varchar(50),p_idcl varchar(10), p_soLuong float, p_giaMua float, p_giaBan float, p_ghiChu varchar(200), p_idHang varchar(50))
begin 
	update hanghoa set tenhang = p_tenHang, id_cl = p_idcl, soluong = p_soLuong, giamua = p_giaMua, giaban = p_giaBan, ghichu = p_ghiChu where id_hang = p_idHang;
end $$
DELIMITER 

#drop procedure SP_UpdateProduct;

delimiter $$
create procedure SP_InsertProduct(p_idHang varchar(50), p_tenHang varchar(50), 
p_idChatlieu varchar(50), p_soLuong float, p_giaMua float, p_giaBan float, p_ghiChu varchar(200))
begin 
	insert into hanghoa values (p_idHang, p_tenHang, p_idChatlieu, p_soLuong, p_giaMua, p_giaBan, p_ghiChu);
end $$
DELIMITER 
call SP_InsertProduct(p_idHang, p_tenHang, p_idChatlieu, p_soLuong, p_giaMua, p_giaBan, p_ghiChu);

call SP_UpdateProduct('Quan xi ren lua','k03',1000,50000,100000,'quan xi sexy','A05');
-- call SP_UpdateProduct(@proName, @proQuantily, @proPurPrice, @proSalePrice, @proDes, @proID);

delimiter $$
create procedure SP_DeleteProduct(p_idhang varchar(50))
begin
	Delete from hanghoa where id_hang = p_idhang;
end $$
delimiter

call SP_DeleteProduct('A07');

update hanghoa set tenhang = 'Ao Somi bau', soluong = 100, giamua = 120000, giaban = 200000, ghichu = 'Ao tay ngan' where id_hang = 'A01';

Insert into RegisterUser(lastName, userName, password, email, type) values ('Trung Nghia', 'Nghia', '1', 'Nghia@gmail.com','1');

delimiter $$
create procedure AddUser(in _lastnName varchar(50), in _userName varchar(50), in _passWord varchar(50), in _email varchar(50), in _role int, in _description nvarchar(200))
begin
	insert into RegisterUser(lastName, userName, passWord, email, type, userDescription) values (_lastnName,_userName,_passWord,_email,_role,_description);
end $$
delimiter 

call ShowUser();

call AddUser("ttanh","tienanh",1,"ttanh@gmail.com",0);

#call SP_ShowMaterial();

#drop procedure AddUser;
call ShowCustomer();
select * from RegisterUser;

update RegisterUser set userName = 'admin' where id = '4';



delimiter $$
create procedure SP_UpdateCustomer(p_tenKhach varchar(50), p_diaChi varchar(100), p_sdt varchar(15), p_id_kh varchar(10))
begin 
	update khach set tenkhach = p_tenKhach , diachi = p_diaChi , sdt  = p_sdt where id_kh = p_id_kh;
end $$
DELIMITER 

delimiter $$
create procedure SP_DeleteCustomer(p_id_kh varchar(10))
begin 
	delete from khach where id_kh = p_id_kh;
end $$
DELIMITER 
select * from chitiethdban;
select *from hdban;
call SP_DeleteCustomer('kh009');
call SP_UpdateCustomer('Duong Qua','Nam Tong','0919900000','kh002');


-----------------------------------------------------------------------------------------------------------------------
select sum(tongtien)  as 'Tong tien' from HDban
    where ngayban = '2019-11-15';
    select sum(tongtien) tongtien from HDban;
    select * from HDban;
-- --------------------------------------------------------------------------------------------------------------------

delimiter $$
create function tongdoanhthu() returns float
reads sql data
begin
	declare tong float;
	 select sum(tongtien) into tong from HDban;
     return tong;
end $$
select * from hdBan
drop function tongdoanhthu

-- delimiter;
-- goi ham
select tongdoanhthu() as 'Tong doanh thu';

---------------------------------------------------------------------------------------------------------------------------
-- 1 show tất cả thông tin của 1 hàng hóa thông qua ID hàng hóa được cung cấp 

delimiter $$
create procedure ProductInformation(pid_hang varchar(10))
begin
	select hh.tenhang, hh.soluong, cl.ten_cl, hh.giaban,hh.ghichu from hanghoa hh inner join chatlieu cl
	on hh.id_cl=cl.id_cl where hh.id_hang= pid_hang;
end $$
delimiter

call ProductInformation('vi02');

-- 2 show tất cả thông tin của 1 Khách thông qua ID khách được cung cấp

delimiter $$ 
create procedure CustomerInformation(pid_kh varchar(10))
begin
	select kh.tenkhach, kh.diachi, kh.sdt, hd.id_hd 
    from khach kh inner join Hdban hd
    on kh.id_kh=hd.id_kh
    where kh.id_kh = pid_kh; 
    
end $$

delimiter
call CustomerInformation('1');
    
-- 3 show tất cả thông tin của 1 nhân viên thông qua ID nhân viên được cung cấp

delimiter $$
create procedure EmployeeInformation(pid int)
begin
	select nv.id,nv.lastname, nv.email, nv.type
    from registerUser nv inner join HDban hd
    on nv.id=hd.id
    where id=pid;
end $$


delimiter $$ 
create procedure GetUserName(pid_cl varchar(50))
begin
	
    select registerUser.lastName from registerUser,hdban where registerUser.id = 1;

    #select * from HDban where id_hd = "hd001" @billID
end $$

#call ShowProduce()
drop procedure EmployeeInformation;

call EmployeeInformation(1);
delimiter
  select * from khach
  select * from HDban;
  select * from registerUser;
  
-- 4 show tất cả thông tin của 1 chât liệu thông qua ID chất liệu được cung cấp

delimiter $$ 
create procedure MaterialInformation(pid_cl varchar(50))
begin
	select cl.ten_cl, hh.tenhang, hh.ghichu
    from chatlieu cl inner join hanghoa hh
    where cl.id_cl=pid_cl;
end $$

call MaterialInformation('ct01');
delimiter

DELIMITER ;;
CREATE PROCEDURE `SP_InvoiceInfo`(p_id_hd varchar(50))
begin 
	SELECT a.id_hang as 'ID Mat hang', b.tenhang as 'Ten hang', a.soluong as 'So luong', b.giaban as 'Gia ban', a.giamgia as 'Giam gia', a.thanhtien as 'Thanh tien' 
    FROM chitietHDban AS a, hanghoa AS b WHERE a.id_hd = p_id_hd AND a.id_hang=b.id_hang;
end ;;
DELIMITER ;

DELIMITER ;;
create PROCEDURE `SP_NewInvoice`(p_id_hd varchar(50), p_id_nv int, p_ngayban nvarchar(50), p_id_kh varchar(50), p_tongtien float)
begin 
	insert into  HDban(id_hd, id_nv, ngayban, id_kh, tongtien) values ( p_id_hd, p_id_nv, p_ngayban, p_id_kh, p_tongtien);
end ;;
DELIMITER ;

drop procedure SP_NewInvoice;

Call SP_NewInvoice( @id_hd , @id_nv , @date , @id_kh , @tongtien )

DELIMITER ;;
CREATE PROCEDURE `SP_NewInvoiceInfo`(p_id_hd varchar(50), p_id_hang varchar(50), p_soluong float, p_donGia float, p_giamgia float, p_thanhtien float)
begin 
	insert into  chitietHDban(id_hd, id_hang, soluong, dongia, giamgia, thanhtien) values ( p_id_hd, p_id_hang, p_soluong, p_donGia, p_giamgia, p_thanhtien);
end ;;
DELIMITER ;

DELIMITER ;;
CREATE PROCEDURE `SP_UpdatePriceInvoice`(p_tongtien float, p_id_hd varchar(50))
begin 
	update HDban set tongtien = p_tongtien where id_hd = p_id_hd;
end ;;
DELIMITER ;

DELIMITER ;;
CREATE PROCEDURE `SP_UpdateProductQuantily`(p_soluong float, p_id_hang varchar(50))
begin 
	update hanghoa set soluong = p_soluong where id_hang = p_id_hang;
end ;;
DELIMITER ;
select * from hdban

DELIMITER ;;
CREATE PROCEDURE `SP_UpdateMaterial`(p_idcl varchar(50), p_tencl varchar(50))
begin 
	Update chatlieu set ten_cl = p_tencl where id_cl = p_idcl ;
end ;;
DELIMITER ;

DELIMITER ;;
CREATE PROCEDURE `SP_InsertMaterial`(p_idcl varchar(50), p_tencl varchar(50))
begin 
	Insert into chatlieu values ( p_idcl , p_tencl );
end ;;
DELIMITER ;

DELIMITER ;;
CREATE PROCEDURE `SP_DeleteMaterial`(p_idcl varchar(50))
begin 
	Delete from chatlieu where id_cl = p_idcl;
end ;;
DELIMITER ;


call SP_DeleteMaterial("v03");

call SP_InsertMaterial("v02","voan nhan tao");

call SP_UpdateMaterial("da01","da ca sauuu");
drop procedure SP_UpdateMaterial;
select * from hanghoa;

DELIMITER ;;
CREATE PROCEDURE `SP_NewCustomer`(p_tenKhach varchar(50), p_diaChi varchar(100), p_sdt varchar(15))
begin 
	insert into khach (tenkhach, diachi, sdt) value( p_tenKhach, p_diaChi ,p_sdt);
end ;;
DELIMITER ;
#select * from khach;
call SP_NewCustomer("NgoKo","Sugar","09099990099");

#select distinct id_hd from chitietHDban;
#select * from chitietHdban;
#select * from hdban;
#call ShowProduce;

/* -- -----------------------------------------------------------
 -- delimiter $$
create function soHD() return int
reads sql data
begin
	declare soHD float;
	 select count(id_hd)  from HDban into soHD;
     return soHD;
end $$
  */