"use client";
import { useRouter } from "next/navigation";

const PublishOfferButton = ({ companyId, branchId }: { companyId: string; branchId: string }) => {
  const router = useRouter();

  return (
    <button
      onClick={() => router.push(`/companies/${companyId}/${branchId}/publishOffer`)}
    >
      Publish Offer
    </button>
  );
};

export default PublishOfferButton;
