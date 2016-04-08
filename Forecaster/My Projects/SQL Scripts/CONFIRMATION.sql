SELECT       c.CustomerAccountNum, pe.PromotionNumber, pe.Active as PROMOTION_ACTIVE
	, p.TypeID as PERFORMANCE_TYPEID, t.Type as PERFORMANCE_TYPE, P.Quantity, P.Price,P.StartDate, P.EndDate, P.Active as PERFORMANCE_ACTIVE
	,li.StatusID as LINE_ITEM_STATUSID, s.Status as LINE_ITEM_STATUS, li.ProductGroupEntityKey, li.ConfirmedBy, li.ConfirmedDate, li.Active as LINE_ITEM_ACTIVE
	
FROM              dbo.DBI_FF_PromotionEvent                                   pe 
JOIN              dbo.DBI_FF_Performances                                     p     on pe.ObjectID = p.PromotionEventID
JOIN              dbo.DBI_FF_PerformanceLineItem                              li    on p.objectid = li.PerformanceID
JOIN              dbo.DBI_FF_PerformanceLineItemCustomerXREF                  c     on li.objectid = c.PerformanceLineItemID
JOIN              dbo.DBI_FF_PerformanceLineItemProductXREF                   pr    on li.objectid = pr.PerformanceLineItemID
JOIN              dbo.DBI_FF_ConfirmationStatus                               s     on li.StatusID = s.id
JOIN              dbo.DBI_FF_PerformanceType                                  t     on p.TypeID = t.id

where c.CustomerAccountNum = '004152'
