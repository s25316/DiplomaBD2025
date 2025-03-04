import CreateBranchButton from '@/app/components/buttons/CreateBranchButton'
import React from 'react'
import CreateOfferTemplateButton from '@/app/components/buttons/CreateOfferTemplateButton'
import { getServerSession } from 'next-auth'
import { authOptions } from '@/app/api/auth/[...nextauth]/route'
import Link from 'next/link'

interface BranchesProfile{
  branchId: string,
  name: string
}

interface Templates{
  offerTemplateId: string,
  name: string
}

const CompanyDetails = async ({ params, }:{
  params: Promise<{ id: string}>
}) => {
  const session = await getServerSession(authOptions)
  const { id } = await params

  var branches: BranchesProfile[] = []
  let offerTemplates: Templates[] = [];

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


    const templatesRes = await fetch(
      `http://localhost:8080/api/User/companies/${id}/offerTemplates`,
      {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${session?.user.token}`,
        },
      }
    );

    if (templatesRes.ok) {
      const data = await templatesRes.json();
      offerTemplates = data.offerTemplates;
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

        {offerTemplates.length > 0 && (
        <>
          <h2>Offer Templates:</h2>
          <ul>
            {offerTemplates.map((value) => (
              <li key={value.offerTemplateId}>
                <Link href={`/companies/${id}/templates/${value.offerTemplateId}`}>
                  <b>Name: {value.name}</b>
                </Link>
              </li>
            ))}
          </ul>
        </>
        )}

        <br/>
        <CreateBranchButton/>
        <br/>
        <CreateOfferTemplateButton/>
        
    </div>
  )
}

export default CompanyDetails