"use client";
import { useRouter } from "next/navigation";

const PublishOfferButton = ({ companyId, branchId }: { companyId: string; branchId: string }) => {
  const router = useRouter();

  return (
    <button
      onClick={() => router.push(`/companies/${companyId}/${branchId}/publishOffer`)} className="text-blue-600"
    >
      Publish Offer
    </button>
  );
};

export default PublishOfferButton;
