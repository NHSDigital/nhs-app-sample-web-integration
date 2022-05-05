namespace nhsapp.sample.web.integration.NhsLogin
{
    public enum NhsLoginScope
    {
        ScopeWithoutIm1,
        ScopeWithIm1
    }

    public enum NhsLoginVectorsOfTrust
    {
        P5Basic,
        P9Sensitive,
        P5BasicAndP9Sensitive,
    }
}