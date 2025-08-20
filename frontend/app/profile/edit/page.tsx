'use client';

import React, { useEffect, useState } from 'react';
import { useSession } from 'next-auth/react';
import RegularProfileForm from '@/app/components/forms/RegularProfileForm';
import { OuterContainer } from '@/app/components/layout/PageContainers';

interface SkillType {
  skillTypeId: number;
  name: string;
}

interface Skill {
  skillId: number;
  name: string;
  skillType: SkillType;
}

interface UrlType {
  urlTypeId: number;
  name: string;
}

interface Url {
  value: string;
  urlType: UrlType;
}

interface Address {
  countryId: number;
  countryName: string;
  stateId: number;
  stateName: string;
  cityId: number;
  cityName: string;
  streetId: number | null;
  streetName: string | null;
  houseNumber: string;
  apartmentNumber: string | null;
  postCode: string;
  lon: number;
  lat: number;
}

export interface UserProfile {
  logo: string | null;
  name: string;
  surname: string;
  description: string | null;
  phoneNum: string;
  contactEmail: string;
  birthDate: string;
  isTwoFactorAuth: boolean;
  isIndividual: boolean;
  isStudent: boolean;
  isAdmin: boolean;
  created: string;
  blocked: string | null;
  removed: string | null;
  skills: Skill[];
  urls: Url[];
  address: Address | null;
}

interface UserApiResponse {
    personPerspective: UserProfile;
    // companyPerspective: any; 
}
const EditProfilePage = () => {
  const { data: session } = useSession();
  const [userData, setUserData] = useState<UserProfile | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  useEffect(() => {
    const fetchUser = async () => {
      if (!session?.user?.token) {
          setLoading(false);
          return;
      }
      try {
      const res = await fetch(`${backUrl}/api/User`, {
        headers: {
          Authorization: `Bearer ${session.user.token}`,
        },
      });
      const data: UserApiResponse = await res.json();
      setUserData(data.personPerspective);
      }catch (err) {
        if(err instanceof Error)
          setError(err.message);
      } finally {
          setLoading(false);
      }
    };

    fetchUser();
  }, [session]);

  if (!session?.user?.token) return <p>Unauthorized</p>;
  if (loading) return <p>Loading...</p>;
  if (error) return <p className="text-center p-4 text-red-500">Error: {error}</p>;
  if (!userData) return <p>No data found.</p>;

  return (
    <OuterContainer className="max-w-xl mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6 text-center">Edit Profile</h1>
      <RegularProfileForm token={session.user.token} initialData={userData} />
    </OuterContainer>
  );
};

export default EditProfilePage;