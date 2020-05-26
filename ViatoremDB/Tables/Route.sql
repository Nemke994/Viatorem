CREATE TABLE [dbo].[Route]
(
	[Route_id]			UNIQUEIDENTIFIER	NOT NULL PRIMARY KEY, 
    [Start_location]	UNIQUEIDENTIFIER	NOT NULL, 
    [End_location]		UNIQUEIDENTIFIER	NOT NULL

	CONSTRAINT [FK_Route_Start_location]		 FOREIGN KEY ([Start_location])		REFERENCES [Location]([Location_id]), 
	CONSTRAINT [FK_Route_End_location]			 FOREIGN KEY ([End_location])		REFERENCES [Location]([Location_id]), 
)
