'use client';
import { useRouter } from 'next/navigation';
import { useSession } from 'next-auth/react';

const DeleteTemplateButton = ({
  offerTemplateId,
  companyId,
}: {
  offerTemplateId: string;
  companyId: string;
}) => {
  const { data: session } = useSession();
  const router = useRouter();

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  const handleDelete = async () => {
    const confirmed = confirm('Are you sure you want to delete this template?');
    if (!confirmed || !session?.user?.token) return;

    const res = await fetch(
      `${backUrl}/api/CompanyUser/companies/offerTemplates/${offerTemplateId}`,
      {
        method: 'DELETE',
        headers: {
          Authorization: `Bearer ${session.user.token}`,
        },
      }
    );

    if (res.ok) {
      router.push(`/companies/${companyId}`);
    } else {
      alert('Failed to delete the template.');
    }
  };

  return (
    <button
      onClick={handleDelete}
      className="mt-2 bg-red-500 text-white px-4 py-2 rounded"
    >
      Delete Template
    </button>
  );
};

export default DeleteTemplateButton;