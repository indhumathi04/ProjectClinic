--CREATE DATABASE------------------------------------------------------------------------------------------------------------------------------------
create database clinicDB

use clinicDB

--CREATE TABLES--------------------------------------------------------------------------------------------------------------------------------
create table staff(staffUserName varchar(10) not null unique,firstName varchar(20) not null,lastName varchar(20),staffPassword varchar(15) not null);
select * from staff
delete from staff

create table patient(patientId int identity(1000,1), firstName varchar(20) not null,lastName varchar(20), sex varchar(8),age int,dob date,check(sex in ('male', 'female', 'unknown')))
select * from patient
delete from patient


create table doctor(doctorId int identity(3000,1),firstName varchar(20) not null,lastName varchar(20),sex varchar(8),specialization varchar(20) not null,visitStartTime time,visitEndTime time,check(sex in ('male', 'female', 'unknown')))
select * from doctor


create table appointments(appointmentId int identity(1,1),patientId int,specialization varchar(20),doctorId int,visitDate date format 'dd/', appointmentTime time,appointmentStatus varchar(15))
drop table appointments

--INSERT DATA IN TABLES------------------------------------------------------------------------------------------------------------------------------

insert into staff values('staff01','Indhu','V','pass@staff')

insert into doctor(firstName,lastName,sex,specialization,visitStartTime,visitEndTime) values('John','Smith','male','General','01:00pm','2:00pm'),('Simba','Mufasa','male','Internal Medicine','6:00pm','8:00pm')
insert into doctor(firstName,lastName,sex,specialization,visitStartTime,visitEndTime) values('Hannah','Baker','female','Ophthalmology','10:00am','12:00pm'),('Devi','vishwakumar','female','Orthopedics','7:00pm','8:00pm')


insert into appointments(patientId,specialization,doctorId,visitDate,appointmentTime,appointmentStatus) values(1002,'Ophthalmology',3002,'07/09/2022','10:00am','booked'),(1002,'Ophthalmology',3002,'07/09/2022','11:00am','cancelled')


