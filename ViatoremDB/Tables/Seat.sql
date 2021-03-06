﻿CREATE TABLE [dbo].[Seat]
(
	[Seat_id]		UNIQUEIDENTIFIER	NOT NULL	PRIMARY KEY, 
    [Bus_id]		UNIQUEIDENTIFIER	NOT NULL, 
    [Reserved]		BIT					NOT NULL	DEFAULT 0

	CONSTRAINT [FK_Seat_Bus]		 FOREIGN KEY ([Bus_id])			REFERENCES [Bus]([Bus_id]), 
)
