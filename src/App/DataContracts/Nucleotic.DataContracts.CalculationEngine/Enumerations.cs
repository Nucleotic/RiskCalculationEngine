namespace Nucleotic.DataContracts.CalculationEngine
{
    #region Global Enums

    public enum TestMode
    {
        EXTERNAL,
        INTERNAL
    }

    /// <summary>
    ///     All Risks
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum AllRiskType
    {
        Antiques,
        PedalCycles,
        Binoculars,
        CarRadios,
        CellPhone,
        CellularPhones,
        ContactLenses,
        DivingAndSportEquipment,
        GeneralProtection,
        GoldCoins,
        GoldCoinsBankVault,
        GolfCart,
        GolfClubs,
        HearingAid,
        Jewellery,
        JewelleryBankVault,
        LaptopsAndNotebooks,
        MusicInstrument,
        Other,
        PhotographicEquipment,
        QuadBikes,
        RadioAndCDs,
        SpecifiedItem,
        Spectacles,
        Tools,
        Unspecified,
        UnspecifiedGeneralProtection
    }

    /// <summary>
    ///     Calculation types enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum CalculationTypes
    {
        Loading,
        CoverAddition,
        ExcessWaiver,
        FlatRate
    }

    /// <summary>
    ///     Calculator Function types enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum CalculatorFunctionType
    {
        AdditionalExcess,
        AdditionalExcessWaiver,
        Age,
        AnnualisedBasicExcessWaiver,
        AnnualisedFullLossExcessWaiver,
        BasicExcessWaiver,
        BrokerIncrease,
        BrokerLoadings,
        BuildingSecurity,
        BuildingType,
        CarHireCover,
        ClaimFrequency,
        ClaimTotal,
        ClaimType,
        ConstructionType,
        CoverType,
        CreditShortfall,
        CrestaArea,
        DiscretionaryFloorDiscount,
        FlatRate,
        FleetCoverType,
        FullLossExcessWaiver,
        GoodsInTransit,
        MinimumAmount,
        Navigate,
        OccupancyType,
        RiskType,
        RoofType,
        SubsistenceLandSlip,
        VehicleLoadings,
        VehicleMakeModel,
        VehicleSecurity,
        VehicleType,
        VehicleUse,
        WallType,
        Fleet,
        FleetGoodsInTransit
    }

    /// <summary>
    ///     Claim types enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum ClaimType
    {
        ThirdParty,
        AccidentalDamage,
        ArmedRobbery,
        ArmedRoberyFromHome,
        Burglary,
        BurstGeyserPipes,
        DataReinstatement,
        Earthquake,
        Fire,
        Flood,
        Hail,
        HailDamage,
        Hijack,
        ImpactDamage,
        IncompatibleSoftware,
        ItemsAwayFromYourPremises,
        Laptop,
        Lightning,
        LossOfItem,
        MaliciousDamage,
        Other,
        OwnDamage,
        PowerSurge,
        StormDamage,
        SubsidenceLandslip,
        Theft,
        TheftFromAnotherPremises,
        TheftFromHomeForcedEntry,
        TheftFromHomeNoForcedEntry,
        TheftFromVehicle,
        WaterDamage,
        WindDamage,
        Windscreen,
        Unknown
    }

    /// <summary>
    ///     RuleTypes in data enumeration
    /// </summary>
    public enum DataRuleTypes
    {
        Banding,
        BandingExtended,
        Attributes,
        OwnTable,
        AttributesExtended,
        RuleExtended,
        AttributesBanding,
        AttributesBandingExtended
    }

    /// <summary>
    ///     Engine Type enumeration
    /// </summary>
    public enum EngineType
    {
        MotorLoadings = 1,
        ExcessWaivers = 2,
        BuildingLoadings = 3,
        FlatRateLoadings = 4,
        AllRiskLoadings = 5,
        ContentLoadings = 6,
    }


    /// <summary>
    ///     Product type enumeration
    /// </summary>
    public enum ProductType
    {
        Domestic,
        Commercial
    }

    /// <summary>
    ///     Risk Item cover type enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum RiskCoverType
    {
        Comprehensive,
        ThirdPartyFireAndTheft,
        ThirdPartyOnly,
        FireAndTheft,
        ExcludeTheft,
        Unknown
    }

    /// <summary>
    ///     Policy type enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum RiskItemRatingType
    {
        NoneSpecified = 0,
        MotorVehicle = 1,
        Buildings = 2,
        AllRisk = 3,
        HouseholdContents = 4,
        FlatRate = 6
    }
    /// <summary>
    ///     Scale and extent of asset use enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum ScaleOfUse
    {
        Private,
        Business,
        PrivateAndBusiness
    }

    /// <summary>
    ///     Security features enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum SecurityFeatures
    {
        AccessControl,
        AntiSmashFilm,
        BarkingDog,
        BurglarBars,
        FactoryImmobiliser,
        LinkedAlarm,
        SecurityGates,
        Tracker,
        VssImmobiliser,
    }

    #endregion

    #region Motor Enums

    /// <summary>
    ///     Vehicle type enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum VehicleType
    {
        FourByFourVehicle,
        Bus,
        LDV,
        Minibus,
        MotorCycle,
        Motorhome,
        PanelVan,
        PassengerVehicle,
        Passenger,
        QuadBike,
        SUV,
        Tractor,
        Trike,
        Truck
    }

    #endregion

    #region Flat Rate Enums

    /// <summary>
    ///     Flat rate calculation types enumeration
    ///     TODO: Enumerate from Database
    /// </summary>
    public enum FlatRateCalculationTypes
    {
        None,
        GoodsInTransit,
        CoverAdditions,
        Fleet,
        FleetGoodsInTransit
    }

    #endregion
}