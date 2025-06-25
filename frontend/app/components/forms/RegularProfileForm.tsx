'use client';
import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { GeocoderAutocomplete } from '@geoapify/geocoder-autocomplete';
import '@geoapify/geocoder-autocomplete/styles/round-borders.css';
import { UserProfile } from '@/app/profile/edit/page';

interface RegularProfileFormProps {
  initialData: UserProfile;
  token: string;
}

interface SkillOption {
    skillId: number;
    name: string;
}

interface UrlTypeOption {
    urlTypeId: number;
    name: string;
}

interface FormUrl {
    value: string;
    urlTypeId: number;
}

interface ProfileUpdatePayload {
    description: string;
    contactEmail: string;
    contactPhoneNumber: string;
    birthDate: string;
    isTwoFactorAuthentication: boolean;
    isStudent: boolean;
    skillsIds: number[];
    urls: FormUrl[];
    address: {
        countryName: string;
        stateName: string;
        cityName: string;
        streetName: string;
        houseNumber: string;
        apartmentNumber: string | null;
        postCode: string;
        lon: number;
        lat: number;
    } | null;
}

const RegularProfileForm = ({ initialData, token }: RegularProfileFormProps) => {

  const [skills, setSkills] = useState<SkillOption[]>([]);
  const [urlTypes, setUrlTypes] = useState<UrlTypeOption[]>([]);
  const router = useRouter();

  const [form, setForm] = useState<ProfileUpdatePayload>({
    description: initialData.description || '',
    contactEmail: initialData.contactEmail || '',
    contactPhoneNumber: initialData.phoneNum || '',
    birthDate: initialData.birthDate?.substring(0, 10) || '',
    isTwoFactorAuthentication: initialData.isTwoFactorAuth || false,
    isStudent: initialData.isStudent || false,
    skillsIds: initialData.skills?.map(s => s.skillId) || [],
    urls: initialData.urls?.map(u => ({ value: u.value, urlTypeId: u.urlType.urlTypeId })) || [],
    address: initialData.address ? {
      countryName: initialData.address?.countryName || '',
      stateName: initialData.address?.stateName || '',
      cityName: initialData.address?.cityName || '',
      streetName: initialData.address?.streetName || '',
      houseNumber: initialData.address?.houseNumber || '',
      apartmentNumber: initialData.address?.apartmentNumber || '',
      postCode: initialData.address?.postCode || '',
      lon: initialData.address?.lon || 0,
      lat: initialData.address?.lat || 0,
    }: null,
  });
const setAddress = (newAddressPart: Partial<ProfileUpdatePayload['address']>) => {
    setForm(prevForm => {
      const currentAddress = prevForm.address || {
        countryName: '',
        stateName: '',
        cityName: '',
        streetName: '',
        houseNumber: '',
        apartmentNumber: null,
        postCode: '',
        lon: 0,
        lat: 0,
      };

      return {
        ...prevForm,
        address: {
          ...currentAddress, // keep existing address
          ...newAddressPart, // override/add new address part
        },
      };
    });
  };
  
  useEffect(() => {
    const headers = { Authorization: `Bearer ${token}` };
    Promise.all([
      fetch('http://localhost:8080/api/Dictionaries/skills', { headers }),
      fetch('http://localhost:8080/api/Dictionaries/urlTypes', { headers }),
    ]).then(async ([skillsRes, urlTypesRes]) => {
      setSkills(await skillsRes.json());
      setUrlTypes(await urlTypesRes.json());
    });

    const apiKey = process.env.GEOAPIFY_API!;
    const container = document.getElementById('autocomplete-container');

    if (container && !container.children.length) {
      const geo = new GeocoderAutocomplete(container, apiKey, {
        lang: 'en',
        filter: { countrycode: ['pl'] },
      });

      geo.on('select', (location) => {
        const props = location.properties;
        setForm(prev => ({ ...prev, address: {
          countryName: props.country || '',
          stateName: props.state || '',
          cityName: props.city || '',
          streetName: props.street || '',
          houseNumber: props.housenumber || '',
          apartmentNumber: '',
          postCode: props.postcode || '',
          lon: props.lon,
          lat: props.lat,
        }}));
      });
    }
  }, [token]);

  const updateUrl = (index: number, field: keyof FormUrl, value: string | number) => {
    const updatedUrls = [...form.urls];
    updatedUrls[index] = { ...updatedUrls[index], [field]: value };
    setForm(prev => ({ ...prev, urls: updatedUrls }));
  };

  const addUrl = () => {
    setForm(prev => ({...prev, urls: [...prev.urls, { value: '', urlTypeId: 1 }]}));
  }

  const removeUrl = (index: number) => {
    setForm(prev => ({ ...prev, urls: prev.urls.filter((_, i) => i !== index)}));
  };

  const handleSkillChange = (skillId: number, isSelected: boolean) => {
      setForm(prev => ({
          ...prev,
          skillsIds: isSelected
            ? [...prev.skillsIds, skillId]
            : prev.skillsIds.filter(id => id !== skillId)
      }));
  };

  const handleSubmit = async () => {
    const deduplicatedUrls = form.urls
      .filter((u : { value: string }) => u.value.trim() !== '')
      .filter((value: { value: string; urlTypeId: number }, index: number, self: { value: string; urlTypeId: number }[]) =>
        index === self.findIndex((v) => v.value === value.value && v.urlTypeId === value.urlTypeId)
      );

    const fullPayload: ProfileUpdatePayload = { ...form, urls: deduplicatedUrls };

    const res = await fetch('http://localhost:8080/api/User/regularData', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(fullPayload),
    });


    if (res.ok){
      alert('Profile updated!');
      router.push('/profile');
    }
    else alert('Failed to update profile');
  };

  return (
    <div className="flex flex-col gap-4 mt-4 max-w-2xl border rounded-lg">
      <label><b>Description</b></label>
      <textarea className='border border-gray-300' value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />

      <label><b>Contact Email</b></label>
      <input className='border border-gray-300 rounded-md p-1' value={form.contactEmail} onChange={(e) => setForm({ ...form, contactEmail: e.target.value })} />

      <label><b>Phone Number</b></label>
      <input className='border border-gray-300 rounded-md p-1' value={form.contactPhoneNumber} onChange={(e) => setForm({ ...form, contactPhoneNumber: e.target.value })} />

      <label><b>Birth Date</b></label>
      <input className='border border-gray-300 rounded-md p-1' type="date" value={form.birthDate} onChange={(e) => setForm({ ...form, birthDate: e.target.value })} />
      {(form.address?.countryName != "") && (
        <div className="text-sm text-gray-700 italic mb-2">
          <b>Current address:</b><br />

        {[
          "ul.",
          form.address?.streetName,
          form.address?.houseNumber, "/",
          form.address?.apartmentNumber,",",
          form.address?.postCode, ",",
          form.address?.cityName,
          form.address?.countryName,
        ]
          .filter(Boolean)
          .join(' ')}
      </div>
      )}

      <div id="autocomplete-container" style={{ position: 'relative' }} />
      <label>Apartment Number</label>
      <input
        type="text"
        className='border border-gray-300 rounded-md p-1'
        value={form.address?.apartmentNumber || ''}
        onChange={(e) => setAddress({ ...form.address, apartmentNumber: e.target.value })}
      />
      
      <label>
        <input
          type="checkbox"
          checked={form.isTwoFactorAuthentication}
          onChange={(e) => setForm({ ...form, isTwoFactorAuthentication: e.target.checked })}
        /> Two-Factor Authentication
      </label>

      <label>
        <input
          type="checkbox"
          checked={form.isStudent}
          onChange={(e) => setForm({ ...form, isStudent: e.target.checked })}
        /> Are you a student?
      </label>

       <label><b>Skills</b></label>
       <div className="grid grid-cols-2 md:grid-cols-3 gap-2">
         {skills.map(skill => (
          <label key={skill.skillId} className="text-sm flex items-center gap-2">
            <input
              type="checkbox"
              checked={form.skillsIds.includes(skill.skillId)}
              onChange={(e) => handleSkillChange(skill.skillId, e.target.checked)}
            />
            {skill.name}
          </label>
        ))}
      </div>

      <label><b>Links</b></label>
      {form.urls.map((url, index) => (
        <div key={index} className="flex gap-2 items-center">
          <input className='border border-gray-300 rounded-md p-1' type="text" value={url.value} onChange={(e) => updateUrl(index, 'value', e.target.value)} />
          <select className='border border-gray-300 rounded-md p-2' value={url.urlTypeId} onChange={(e) => updateUrl(index, 'urlTypeId', Number(e.target.value))}>
            {urlTypes.map(t => <option key={t.urlTypeId} value={t.urlTypeId}>{t.name}</option>)}
          </select>
          <button
            className = 'bg-red-600 text-white rounded-lg hover:bg-red-700'
            type="button" onClick={() => removeUrl(index)}>Remove</button>
        </div>
      ))}
      <button
        className = 'bg-green-600 text-white py-2 px-4 rounded mt-4'
        type="button" onClick={addUrl}>Add Link
      </button>

      <button
        onClick={handleSubmit}
        className="bg-blue-600 text-white py-2 px-4 rounded mt-4" > Update Profile
      </button>
    </div>
  );
};

export default RegularProfileForm;