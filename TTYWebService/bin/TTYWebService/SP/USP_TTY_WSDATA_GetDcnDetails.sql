/************************************************    
CREATED BY  : Keerthana D
DATE CREATED: 25 Aug 2023
VERSION   :1.0.0.0   
PURPOSE   :Get TTY Get Subprocess
*************************************************/ 
CREATE PROC USP_TTY_WSDATA_GetDcnDetails
@CustId INT,
@ProjId INT,
@FormId INT,
@ProcessId INT,
@SubprocessId INT
AS            
BEGIN      
SET NOCOUNT ON;   
		BEGIN TRY 
		Declare @ShipTypeID Int
		select @ShipTypeID=shiptypeid from shiptype where shipname='TTYWS'
		IF Exists(select Distinct 'Data' from shiptime where custid=@CustId and shiptypeid=@ShipTypeID And  FormId=@FormId and
			convert(varchar,getdate(),108) Between convert(varchar,shiptime,108) And convert(varchar,(DATEADD(mi,gracemins,shiptime)),108))
		Begin
			SELECT DISTINCT FM.CustId,FM.ProjId,FM.FormId,FM.Dcn FROM FILEMASTER FM WITH(NOLOCK)
			WHERE FM.CustId = @CustId AND FM.ProjId = @ProjId AND FM.FormId = @FormId
			AND FM.SubprocessId = @SubprocessId AND FM.SubprocessStatus = 'READY'
		End
		END TRY        
			BEGIN CATCH        
					INSERT INTO ERRORDETAILS(FORMNAME,FUNCTIONNAME,ERRORDESCRIPTION,DATETIME,ERRORTYPE,CUSTID,PROJID) VALUES                                                  
					('USP_TTY_WSDATA_GetDcnDetails',Error_Procedure(),Error_Message(),GETDATE(),'STOREDPROCEDURE',@CustId,@ProjId)       
			END CATCH
SET NOCOUNT OFF;        
END    
