"use client";

import React from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";

const CreateOfferButton = () => {
  const pathname = usePathname();

  return (
    <Link href={`${pathname}/createOffer`}>
      Create Offer
    </Link>
  );
};

export default CreateOfferButton;
