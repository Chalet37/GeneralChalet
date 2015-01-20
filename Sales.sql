create procedure Sell(@CommodityID INTEGER, 
					@Amount INTEGER, 
					@SalesDate DATETIME,
					@EmployeeID INTEGER) 
as
set nocount off
update StorageList
	set StockAmount = StockAmount - @Amount
	where CommodityID = @CommodityID

insert into CashRecord(CommodityID, Amount, UnitPrice, EmployeeID)
values(@CommodityID, 
		@Amount, 
		(select UnitPrice from StorageList where CommodityID = @CommodityID), @EmployeeID)

insert into SalesRecord(CommodityID, SalesAmount, SalesDate, EmployeeID)
values(@CommodityID, @Amount, @SalesDate, @EmployeeID)
go