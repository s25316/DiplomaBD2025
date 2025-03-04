"use client"; // Dodaje "use client", bo `useParams` działa tylko w komponentach klienckich
import React from "react";
import { useParams } from "next/navigation";
import PublishOfferButton from "@/app/components/buttons/PublishOfferButton";

const BranchDetails = () => {
  const params = useParams(); // Pobieram dynamiczne parametry z URL

  if (!params.id || !params.branchId) {
    return <div>Loading...</div>; //Gdy params nie są jeszcze dostępne
  }

  return (
    <div>
      <h1>Branch Details</h1>
      <br />
      <PublishOfferButton companyId={params.id as string} branchId={params.branchId as string} />
    </div>
  );
};

export default BranchDetails;
