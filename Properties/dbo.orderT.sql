CREATE TABLE [dbo].[orderT] (
    [orderId]      INT           NOT NULL,
    [clientId]     INT           NULL,
    [carModel]     NVARCHAR(50) NULL,
    [workType]     INT           NULL,
    [price]        MONEY         NULL,
    [employeeName] INT           NULL,
    [completed]    TINYINT       NULL,
    PRIMARY KEY CLUSTERED ([orderId] ASC)
);

