import CreateBranchButton from '@/app/components/buttons/CreateBranchButton'
import React from 'react'
import CreateOfferTemplateButton from '@/app/components/buttons/CreateOfferTemplateButton'
import { getServerSession } from 'next-auth'
import { authOptions } from '@/app/api/auth/[...nextauth]/route'
import { useParams } from 'next/navigation'
import Link from 'next/link'

interface BranchesProfile{
  branchId: string,
  name: string
}

const CompanyDetails = async ({ params, }:{
  params: Promise<{ id: string}>
}) => {
  const session = await getServerSession(authOptions)
  const { id } = await params

  var branches: BranchesProfile[] = []

  if(session?.user.token){
    const res = await fetch (`http://localhost:8080/api/User/companies/${id}/branches`, {
      headers:{
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user.token}`
      }
    })
    if(res.ok){
      let tmp = await res.json()
      branches = tmp.branches
    }
  }

  return (
    <div>
        CompanyDetails
        {branches &&
        <>
        <h2>Branches:</h2>
          <ul>
            {branches.map((value) => 
            <li key={value.branchId}><Link href={`/companies/${id}/${value.branchId}`}><b>Name: {value.name}</b></Link></li>
            )}
          </ul>
        </>
        }
        <br/>
        <CreateBranchButton/>
        <br/>
        <CreateOfferTemplateButton/>
        
    </div>
  )
}

export default CompanyDetails