CREATE TABLE [dbo].[Ticket]
(
	[Ticket_id]		 UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [User_id]		 UNIQUEIDENTIFIER NOT NULL, 
    [Schedule_id]	 UNIQUEIDENTIFIER NOT NULL, 
    [Seat_id]		 UNIQUEIDENTIFIER NOT NULL CONSTRAINT UQ_Ticket_Seat UNIQUE,
    CONSTRAINT [FK_Ticket_User]		 FOREIGN KEY ([User_id])		REFERENCES [User]([User_id]), 
	CONSTRAINT [FK_Ticket_Schedule]	 FOREIGN KEY ([Schedule_id])	REFERENCES [Schedule]([Schedule_id]), 
	CONSTRAINT [FK_Ticket_Seat]		 FOREIGN KEY ([Seat_id])		REFERENCES [Seat]([Seat_id]), 

  
)
