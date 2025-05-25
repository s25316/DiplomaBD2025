'use client';
import React, { useState } from 'react';
import { useSession } from 'next-auth/react';

const EditCompanyForm = ({ company, companyId, onUpdated }: { company: any, companyId: string, onUpdated: (data: any) => void }) => {
  const { data: session } = useSession();
  const [description, setDescription] = useState(company?.description || '');
  const [websiteUrl, setWebsiteUrl] = useState(company?.websiteUrl || '');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!session?.user?.token) return;

    setLoading(true);

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/${companyId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${session.user.token}`,
      },
      body: JSON.stringify({
        description,
        websiteUrl,
        krs: '', // niezmienialne
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
    <form onSubmit={handleSubmit} className="border p-4 mt-6 rounded">
      <h2 className="text-lg font-semibold mb-4">Edit Company</h2>

      <div className="mb-4">
        <label className="block text-sm font-medium mb-1">Description:</label>
        <input
          type="text"
          className="border rounded px-2 py-1 w-full"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
      </div>

      <div className="mb-4">
        <label className="block text-sm font-medium mb-1">Website URL:</label>
        <input
          type="text"
          className="border rounded px-2 py-1 w-full"
          value={websiteUrl}
          onChange={(e) => setWebsiteUrl(e.target.value)}
        />
      </div>

      <button
        type="submit"
        className="bg-blue-600 text-white px-4 py-2 rounded"
        disabled={loading}
      >
        {loading ? 'Saving...' : 'Save Changes'}
      </button>
    </form>
  );
};

export default EditCompanyForm;
