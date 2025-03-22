 // Dodaje "use client", bo `useParams` działa tylko w komponentach klienckich
import React from "react";
// import { useParams } from "next/navigation";
import PublishOfferButton from "@/app/components/buttons/PublishOfferButton";
import { authOptions } from '@/app/api/auth/[...nextauth]/route';
import { getServerSession } from 'next-auth';
import Link from 'next/link';

interface BranchDetails{
  name: string,
  desciption: string,
  created: string,
  address:{
    countryName: string,
    stateName: string,
    cityName: string,
    streetName?: string,
    houseNumber: string,
    apartmentNumber?: string,
    postCode: string,
  }
}

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
    branchData = await res.json().then((data) => data.items[0].branch) as BranchDetails
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
          <p><b>Name:</b> {branchData.name}</p>
          {branchData.desciption && <p><b>Description:</b> {branchData.desciption}</p>}
          <div className="mb-2">
            <p><b>Address:</b></p>
            <p className="ml-4"><b>Country:</b> {branchData.address.countryName}</p>
            <p className="ml-4"><b>City:</b> {branchData.address.cityName}</p>
            {branchData.address.streetName && <p className="ml-4"><b>Street:</b> {branchData.address.streetName}</p>}
            <p className="ml-4"><b>House number:</b> {branchData.address.houseNumber}</p>
            {branchData.address.apartmentNumber && <p className="ml-4"><b>Apartment number:</b> {branchData.address.apartmentNumber}</p>}
          </div>
        </>
      }
      <Link href={`/companies/${id}/${branchId}/publishOffer`}>Publish offer</Link>
      {/* <PublishOfferButton companyId={id as string} branchId={branchId as string} /> */}
    </div>
  );
};

export default BranchDetails;
