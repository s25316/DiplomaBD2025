 // Dodaje "use client", bo `useParams` działa tylko w komponentach klienckich
import React from "react";
// import { useParams } from "next/navigation";
import PublishOfferButton from "@/app/components/buttons/PublishOfferButton";
import { authOptions } from '@/app/api/auth/[...nextauth]/route';
import { getServerSession } from 'next-auth';

const BranchDetails = async ({ params }: { params: { id: string, branchId: string } }) => {
  // const params = useParams(); // Pobieram dynamiczne parametry z URL
  const session = await getServerSession(authOptions);

  //TODO: complete after endpoint change

  const { id, branchId } = await params

  let res = await fetch(`http://localhost:8080/api/CompanyUser/branches/${branchId}`, 
    {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user.token}`,
      }
    }
  )

  var branchData

  if(res.ok){
    branchData = await res.json()
  }

  // if (!params.id || !params.branchId) {
  //   return <div>Loading...</div>; //Gdy params nie są jeszcze dostępne
  // }

  return (
    <div>
      <h1>Branch Details</h1>
      <br />
      {branchData && 
        <>
          Name: {branchData.items[0].branch.name}
          <br />
        </>
      }
      <PublishOfferButton companyId={id as string} branchId={branchId as string} />
    </div>
  );
};

export default BranchDetails;
