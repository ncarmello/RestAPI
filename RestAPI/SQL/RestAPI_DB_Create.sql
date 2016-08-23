USE [master]
GO

/****** Object:  Database [RestAPI]    Script Date: 8/23/2016 12:27:57 PM ******/
CREATE DATABASE [RestAPI]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RestAPI', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\RestAPI.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RestAPI_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\RestAPI_log.ldf' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [RestAPI] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RestAPI].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [RestAPI] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [RestAPI] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [RestAPI] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [RestAPI] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [RestAPI] SET ARITHABORT OFF 
GO

ALTER DATABASE [RestAPI] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [RestAPI] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [RestAPI] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [RestAPI] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [RestAPI] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [RestAPI] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [RestAPI] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [RestAPI] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [RestAPI] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [RestAPI] SET  DISABLE_BROKER 
GO

ALTER DATABASE [RestAPI] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [RestAPI] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [RestAPI] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [RestAPI] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [RestAPI] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [RestAPI] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [RestAPI] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [RestAPI] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [RestAPI] SET  MULTI_USER 
GO

ALTER DATABASE [RestAPI] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [RestAPI] SET DB_CHAINING OFF 
GO

ALTER DATABASE [RestAPI] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [RestAPI] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [RestAPI] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [RestAPI] SET  READ_WRITE 
GO
