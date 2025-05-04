"use client"
import React from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const CreateContractConditionButton = () => {
  return (
    <Link href={`${usePathname()}/createContractCondition`} className="text-blue-600">Create Contract</Link>
  )
}

export default CreateContractConditionButton