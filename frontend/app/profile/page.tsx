'use client';

import React, { useEffect, useState } from 'react';
import { useSession } from 'next-auth/react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import CreateCompanyButton from '@/app/components/buttons/CreateCompanyButton';

interface CompanyProfile {
  companyId: string;
  name: string;
  description: string;
  websiteUrl?: string;
}

const Profile = () => {
  const { data: session } = useSession();
  const router = useRouter();

  const [userData, setUserData] = useState<any | null>(null);
  const [companies, setCompanies] = useState<CompanyProfile[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      if (!session?.user?.token) return;

      try {
        const [userRes, companiesRes] = await Promise.all([
          fetch('http://localhost:8080/api/User', {
            headers: {
              Authorization: `Bearer ${session.user.token}`,
            },
          }),
          fetch('http://localhost:8080/api/CompanyUser/companies', {
            headers: {
              Authorization: `Bearer ${session.user.token}`,
              'Content-Type': 'application/json',
            },
          }),
        ]);

        const userJson = await userRes.json();
        const companiesJson = await companiesRes.json();

        setUserData(userJson.personPerspective);
        setCompanies(companiesJson.items || []);
      } catch (error) {
        console.error('Error loading profile:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [session]);

  if (!session?.user?.token) return <p>Unauthorized</p>;
  if (loading) return <p>Loading...</p>;

  const isFirstTime = !userData?.name;

  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-bold">My Profile</h1>

      {isFirstTime ? (
        <p>No profile data yet.</p>
      ) : (
        <div className="space-y-2">
          <p><b>Name:</b> {userData.name}</p>
          <p><b>Surname:</b> {userData.surname}</p>
          <p><b>Email:</b> {userData.contactEmail}</p>
          <p><b>Phone:</b> {userData.phoneNum}</p>
          <p><b>Birth Date:</b> {userData.birthDate?.substring(0, 10)}</p>

          <p className="mt-4"><b>Skills:</b></p>
          <ul className="list-disc ml-5">
            {userData.skills?.map((s: any) => (
              <li key={s.skillId}>{s.name}</li>
            ))}
          </ul>

          <p className="mt-4"><b>Links:</b></p>
          <ul className="list-disc ml-5">
            {userData.urls?.map((u: any, i: number) => (
              <li key={i}>{u.value} ({u.urlType?.name})</li>
            ))}
          </ul>

          <button
            onClick={() => router.push('/profile/edit')}
            className="mt-6 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            Edit Profile
          </button>
        </div>
      )}

      {/* Companies */}
      <div>
        <h2 className="text-xl font-semibold mt-8 mb-2">My Companies</h2>
        {companies.length > 0 ? (
          <ul className="space-y-2">
            {companies.map((company) => (
              <li key={company.companyId} className="border p-3 rounded max-w-md">
                <Link href={`/companies/${company.companyId}`}>
                  <b>Name:</b> {company.name}
                </Link>
              </li>
            ))}
          </ul>
        ) : (
          <p>You don’t own any companies yet.</p>
        )}
        <div className="mt-4">
          <CreateCompanyButton />
        </div>
      </div>
    </div>
  );
};

export default Profile;
