interface ContractConditions {
    contractConditionId: string;
    companyId: string;
    salaryMin: number,
    salaryMax: number,
    isNegotiable: boolean,
    workModes: {
        contractParameterId: number,
        name: string,
        contractParameterType: {
            contractParameterTypeId: number,
            name: string
        }
    }[]
    employmentTypes: {
        contractParameterId: number,
        name: string,
        contractParameterType: {
            contractParameterTypeId: number,
            name: string
        }
    }[]
    hoursPerTerm: number;
    salaryTerm: {
        name: string
    },
    currency: {
        name: string
    }
}