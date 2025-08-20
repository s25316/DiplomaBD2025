'use client';
import { useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';
import React from 'react';

interface DeleteBranchButtonProps {
  branchId: string;
  companyId: string;
  buttonText?: string;
  className?: string;
  onSuccess?: () => void;
}

const DeleteBranchButton = ({
  branchId,
  companyId,
  buttonText = 'Delete Branch',
  className = 'bg-red-600 text-white px-5 py-2 rounded-lg hover:bg-red-700',
  onSuccess,
}: DeleteBranchButtonProps) => {
  const { data: session } = useSession();
  const router = useRouter();

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  // Custom alert function (reused for consistency)
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
        if(e instanceof Error)
          console.error(e.message)
        alertMessage = message;
      }
    }
    console.log(isError ? "ERROR ALERT:" : "ALERT:", alertMessage);
    alert(alertMessage);
  };

  const handleDelete = async () => {
    const confirmed = window.confirm('Are you sure you want to delete this branch? This action cannot be undone.');
    if (!confirmed || !session?.user?.token) return;

    try {
      const res = await fetch(
        `${backUrl}/api/CompanyUser/companies/branches/${branchId}`,
        {
          method: 'DELETE',
          headers: {
            Authorization: `Bearer ${session.user.token}`,
            'Content-Type': 'application/json',
          },
        }
      );

      if (res.ok) {
        showCustomAlert('Branch deleted successfully!');
        onSuccess?.(); // callback if exist
        router.push(`/companies/${companyId}`);
      } else {
        const errorText = await res.text();
        console.error('Failed to delete branch:', errorText);
        showCustomAlert(`Failed to delete branch: ${errorText}`, true);
      }
    } catch (error) {
      console.error('Error during branch deletion:', error);
      if(error instanceof Error)
      showCustomAlert(`An unexpected error occurred: ${error.message}`, true);
    }
  };

  return (
    <button onClick={handleDelete} className={className}>
      {buttonText}
    </button>
  );
};

export default DeleteBranchButton;
