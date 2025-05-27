"use client"
import React from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const CreateBranchButton = () => {
  return (
    <Link href={`${usePathname()}/createBranch`} className="inline-block mt-1 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">Create new branch</Link>
  )
}

export default CreateBranchButton