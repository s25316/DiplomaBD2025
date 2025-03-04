"use client";
import React from "react";
import { useRouter, useParams } from "next/navigation";

const TemplateDetails = () => {
  const router = useRouter();
  const { id, offerTemplateId } = useParams(); // Pobiera companyId i offerTemplateId z URL-a

  return (
    <div>
      <h1>Template Details</h1>

      <button
        onClick={() => router.push(`/companies/${id}/templates/${offerTemplateId}/offerPublish`)}
      >
        Publish Offer
      </button>
    </div>
  );
};

export default TemplateDetails;
