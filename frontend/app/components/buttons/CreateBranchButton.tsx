"use client"
import React from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const CreateBranchButton = () => {
  return (
    <Link href={`${usePathname()}/createBranch`} className="text-blue-600">Create branch</Link>
  )
}

export default CreateBranchButton