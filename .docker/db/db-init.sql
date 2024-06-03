USE [master]
GO

IF DB_ID('EpicMarketDev') IS NOT NULL
  set noexec on 

CREATE DATABASE [EpicMarketDev];
GO