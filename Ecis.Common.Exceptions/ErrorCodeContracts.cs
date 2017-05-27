namespace ZMH.Common.Exceptions
{
    public class ErrorCodeContract
    {
        public const string JoinSQLStringExceptionCode = "EJS001";
        public const string ExceptionCode = "ER000";
        public const string ApplicationExceptionCode = "ER001";
        public const string ArgumentExceptionCode = "ER002";
        public const string IOExceptionCode = "ER003";
        public const string ApplyDrugExceptionCode = "ER000A";

        #region 患者管理

        /// <summary>
        /// 患者入科
        /// </summary>
        public const string PatInDeptExCode = "ER0001";

        /// <summary>
        /// 入科操作数据库时报错
        /// </summary>
        public const string PatInDeptExecSqlExCode = "ER1001";

        public const string PatInDeptQueryPatExCode = "ER0002";

        #endregion 患者管理

        /// <summary>
        /// 患者流转操作异常
        /// </summary>
        public const string TransferExceptionCode = "DTF001";

        /// <summary>
        /// 记录经治记录失败
        /// </summary>
        public const string SaveTreatRecordWarnCode = "STR001";

        /// <summary>
        /// 记录经治记录错误
        /// </summary>
        public const string SaveTreatRecordExceptionCode = "STR002";

        /// <summary>
        /// 保存用血申请
        /// </summary>
        public const string BloodSaveExCode = "ORD012";

        /// <summary>
        /// 保存和提交医嘱
        /// </summary>
        public const string OrderSaveExCode = "ORD001";

        #region 交接班

        /// <summary>
        /// 新增交接班按钮
        /// </summary>
        public const string HandOverNewExCode = "Error_MH_EC_60_10_10_01";

        /// <summary>
        /// 删除交接班按钮
        /// </summary>
        public const string HandOverDelExCode = "Error_MH_EC_60_10_10_02";

        /// <summary>
        /// 修改交接班按钮
        /// </summary>
        public const string HandOverModifyExCode = "Error_MH_EC_60_10_10_03";

        /// <summary>
        /// 保存交接班按钮
        /// </summary>
        public const string HandOverSaveExCode = "Error_MH_EC_60_10_10_04";

        /// <summary>
        /// 提交交接班按钮
        /// </summary>
        public const string HandOverSubmitExCode = "Error_MH_EC_60_10_10_05";

        /// <summary>
        /// 预览交接班按钮
        /// </summary>
        public const string HandOverViewExCode = "Error_MH_EC_60_10_10_06";

        /// <summary>
        /// 打印交接班按钮
        /// </summary>
        public const string HandOverPrintExCode = "Error_MH_EC_60_10_10_07";

        #endregion
        #region 电子病历

        #endregion

        #region 打印
        public const string PrintExceptionCode = "PE001";
        #endregion

        #region 输液
        public const string InfusionOrder = "IF000";

        public const string InfusionSyncDrugExCode = "IF001";

        public const string InfusionDrugGroup = "IF002";

        public const string InfusionDrugLiquid = "IF003";

        public const string InfusionPuncture = "IF004";

        public const string InfusionBottle = "IF005";

        public const string InfusionInjection = "IF006";

        public const string InfusionPause = "IF007";

        public const string InfusionContinue = "IF008";

        public const string InfusionNeedle = "IF009";

        public const string InfusionEditSpeed = "IF010";

        public const string InfusionPatientEnd = "IF011";

        public const string InfusionSaveDrugExtract = "IF012";

        public const string InfusionDrugCancelGroup = "IF013";

        public const string InfusionSaveOrdersList = "IF014";

        public const string InfusionSaveInfusionRecord = "IF015";

        public const string InfusionMultiple = "IF016";

        public const string InfusionFinish = "IF017";

        public const string InfusionMedicate = "IF018";
        #endregion
    }
}