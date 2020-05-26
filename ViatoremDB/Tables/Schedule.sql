CREATE TABLE [dbo].[Schedule]
(
	[Schedule_id]		UNIQUEIDENTIFIER	NOT NULL PRIMARY KEY, 
    [Route_id]			UNIQUEIDENTIFIER	NOT NULL, 
    [Bus_id]			UNIQUEIDENTIFIER	NOT NULL, 
    [Departure_time]	DATETIME			NOT NULL, 
    [Arrival_time]		DATETIME			NOT NULL

	CONSTRAINT [FK_Schedule_Route]		 FOREIGN KEY ([Route_id])		REFERENCES [Route]([Route_id]), 
	CONSTRAINT [FK_Schedule_Bus]		 FOREIGN KEY ([Bus_id])			REFERENCES [Bus]([Bus_id]), 
)

