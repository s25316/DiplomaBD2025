'use client';
import React, { useState } from 'react';
import { useSession } from 'next-auth/react';
import { InnerSection } from '../layout/PageContainers';

const EditCompanyForm = ({ company, companyId, onUpdated }: { company: Company, companyId: string, onUpdated: (data: Company) => void }) => {
  const { data: session } = useSession();
  const [description, setDescription] = useState(company?.description || '');
  const [websiteUrl, setWebsiteUrl] = useState(company?.websiteUrl || '');
  const [loading, setLoading] = useState(false);

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!session?.user?.token) return;

    setLoading(true);

    const res = await fetch(`${backUrl}/api/CompanyUser/companies/${companyId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify({
        description,
        websiteUrl,
        krs: '', // niezmienne
      }),
    });

    if (res.ok) {
      const json = await res.json();
      onUpdated({ ...company, ...json.item, krs: company.krs });

    } else {
      alert('Update failed.');
    }

    setLoading(false);
  };

  return (
    <form onSubmit={handleSubmit} >
      <InnerSection className="border p-4 mt-6 rounded">
        <h2 className="text-lg font-bold mb-4">Edit Company</h2>

        <div className="mb-4">
          <label className="block text-sm font-semibold mb-1">Description:</label>
          <textarea
            className="global-field-style"
            rows={4}
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-semibold mb-1">Website URL:</label>
          <input
            type="text"
            className="global-field-style"
            value={websiteUrl}
            onChange={(e) => setWebsiteUrl(e.target.value)}
          />
        </div>

        <button
          type="submit"
          className="inline-block mt-2 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
          disabled={loading}
        >
          {loading ? 'Saving...' : 'Save Changes'}
        </button>
      </InnerSection>
    </form>
  );
};

export default EditCompanyForm;
