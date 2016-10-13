SET IDENTITY_INSERT [dbo].[BenefitData] ON
INSERT INTO [dbo].[BenefitData] ([BenefitDataID], [BenefitPlanID], [Value], [Year]) VALUES (2, 1, CAST(2600.0000 AS Decimal(19, 4)), 2016)
INSERT INTO [dbo].[BenefitData] ([BenefitDataID], [BenefitPlanID], [Value], [Year]) VALUES (3, 1, CAST(1700.0000 AS Decimal(19, 4)), 2016)
INSERT INTO [dbo].[BenefitData] ([BenefitDataID], [BenefitPlanID], [Value], [Year]) VALUES (5, 2, CAST(650.0000 AS Decimal(19, 4)), 2016)
INSERT INTO [dbo].[BenefitData] ([BenefitDataID], [BenefitPlanID], [Value], [Year]) VALUES (6, 3, CAST(550.0000 AS Decimal(19, 4)), 2016)
SET IDENTITY_INSERT [dbo].[BenefitData] OFF
