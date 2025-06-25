'use client';
import { useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import React from 'react';

interface DeleteOfferButtonProps {
  offerId: string;
  onSuccess: () => void; // Callback do odświeżenia listy ofert
  buttonText?: string;
  className?: string;
  confirmationMessage?: string;
}

const DeleteOfferButton = ({
  offerId,
  onSuccess,
  buttonText = 'Delete Offer',
  className = 'bg-red-500 text-white px-3 py-1 rounded-md hover:bg-red-600 transition duration-200 shadow-sm',
  confirmationMessage = 'Are you sure you want to delete this offer?',
}: DeleteOfferButtonProps) => {
  const { data: session } = useSession();
  const router = useRouter();

  const showCustomAlert = (message: string, isError: boolean = false) => {
    let alertMessage = message;
    if (isError) {
      try {
        const errorJson = JSON.parse(message);
        if (errorJson.errors) {
          alertMessage = "Validation Errors:\n" + Object.entries(errorJson.errors).map(([key, value]) => `${key}: ${(value as string[]).join(", ")}`).join("\n");
        } else if (errorJson.title) {
          alertMessage = errorJson.title + (errorJson.detail ? `\n${errorJson.detail}` : "");
        } else {
          alertMessage = message;
        }
      } catch (e) {
        alertMessage = message;
      }
    }
    console.log(isError ? "ERROR ALERT:" : "ALERT:", alertMessage);
    alert(alertMessage);
  };

  const handleDelete = async () => {
    const confirmed = window.confirm(confirmationMessage);
    if (!confirmed || !session?.user?.token) return;

    try {
      const res = await fetch(
        `http://localhost:8080/api/CompanyUser/companies/offers/${offerId}`,
        {
          method: 'DELETE',
          headers: {
            Authorization: `Bearer ${session.user.token}`,
            'Content-Type': 'application/json',
          },
        }
      );

      if (res.ok) {
        showCustomAlert('Offer deleted successfully!');
        onSuccess();
      } else {
        const errorText = await res.text();
        console.error('Failed to delete offer:', errorText);
        showCustomAlert(`Failed to delete offer: ${errorText}`, true);
      }
    } catch (error: any) {
      console.error('Error during offer deletion:', error);
      showCustomAlert(`An unexpected error occurred: ${error.message}`, true);
    }
  };

  return (
    <button onClick={handleDelete} className={className}>
      {buttonText}
    </button>
  );
};

export default DeleteOfferButton;