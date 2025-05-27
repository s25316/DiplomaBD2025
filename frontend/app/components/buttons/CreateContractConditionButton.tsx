"use client"
import React from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const CreateContractConditionButton = () => {
  return (
    <Link href={`${usePathname()}/createContractCondition`} className="inline-block mt-2 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">Create new contract</Link>
  )
}

export default CreateContractConditionButton