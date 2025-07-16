'use client';

import React, { useEffect, useState } from 'react';
import { useSession } from 'next-auth/react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import CreateCompanyButton from '@/app/components/buttons/CreateCompanyButton';
import BaseProfileForm from '../components/forms/BaseProfileForm';

interface Company{
  companyId: string;
  name: string;
  description: string;
  websiteUrl?: string;
}
interface SkillInfo {
  name: string;
  skillType: {
    name: string;
  };
}

interface LinkInfo {
  value: string,
  urlType: {
    urlTypeId: number;
    name: string;
  }
}
interface Address {
  streetName: string;
  houseNumber: string;
  apartmentNumber: string | null;
  postCode: string;
  cityName: string;
  countryName: string;
}
interface User {
  logo: string | null;
  name: string;
  surname: string;
  isIndividual: boolean;
  isTwoFactorAuth: boolean;
  isStudent: boolean;
  contactEmail: string;
  phoneNum: string;
  birthDate: string;
  address: Address;
  skills: SkillInfo[];
  urls: LinkInfo[];
}

const Profile = () => {
  const { data: session } = useSession();
  const router = useRouter();

  const [user, setUserDetails] = useState<User | null>(null);
  const [companies, setCompanies] = useState<Company[]>([]);
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

        setUserDetails(userJson.personPerspective);
        setCompanies(companiesJson.items || []);
      } catch (error) {
        console.error('Error loading profile:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [session]);

  if (!session?.user?.token) return <p>Unauthorized/Loading...</p>;
  if (!user) return <p>Loading...</p>;

  const {skills, urls, address} = user;
  const isFirstTime = !user?.name;
  
  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-bold">My Profile</h1>

      {isFirstTime ? (
        <BaseProfileForm
            token={session.user.token}
            onSuccess={() => window.location.reload()}
          />
      ) : (
        <div className="space-y-2">
          <h2><b> {user.isIndividual ? 'Individual account' : 'Company account'} </b></h2>
          <p><b>Name:</b> {user.name}</p>
          <p><b>Surname:</b> {user.surname}</p>
          <p><b>Email:</b> {user.contactEmail}</p>
          <p><b>Phone:</b> {user.phoneNum}</p>
          <p><b>Birth Date:</b> {user.birthDate?.substring(0, 10)}</p>

          {(address !=null) && (
          <p><b>Address:</b> 
            {[
              " ul.",
              address.streetName,
              address.houseNumber, "/",
              address.apartmentNumber, ",",
              address.postCode, ",",
              address.cityName,
              address.countryName,
            ]
              .filter(Boolean)
              .join(' ')}
          </p>
          )}

          <p className="mt-4"><b>Skills:</b></p>
          <ul className="list-disc ml-5">
            {skills?.map((s: any) => (
              <li key={s.skillId}>{s.name}</li>
            ))}
          </ul>

          <p className="mt-4"><b>Links:</b></p>
          <ul className="list-disc ml-5">
            {urls?.map((u: any, i: number) => (
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

      {(user.isIndividual===false) && (
      <div>
        <h2 className="text-xl font-semibold mt-8 mb-2">Companies templates</h2>
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
          <p>You did not register any company template.</p>
        )}
        <div className="mt-4">
          <CreateCompanyButton />
        </div>
      </div>
      )}
    </div>
  );
};

export default Profile;
