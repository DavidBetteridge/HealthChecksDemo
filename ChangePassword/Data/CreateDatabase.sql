create database HealthCheckDemo
go

use HealthCheckDemo
go

create table dbo.Users
(
	Username	 nvarchar(255)	not null primary key,
	PasswordHash nvarchar(300)	not null
)
go

insert into dbo.Users select 'david','something'
go
