'use client';
import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { GeocoderAutocomplete } from '@geoapify/geocoder-autocomplete';
import '@geoapify/geocoder-autocomplete/styles/round-borders.css';
import { UserProfile } from '@/app/profile/edit/page';
import { InnerSection } from '../layout/PageContainers';

interface Skill {
  skillId: number;
  name: string;
  skillType: {
    skillTypeId: number;
    name: string;
  };
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

interface RegularProfileFormProps {
  initialData: UserProfile;
  token: string;
}

const RegularProfileForm = ({ initialData, token }: RegularProfileFormProps) => {
  // const [skills, setSkills] = useState<Skill[]>([]);
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
    } : null,
  });

  const [groupedSkills, setGroupedSkills] = useState<Record<string, Skill[]>>({});

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
          ...currentAddress, 
          ...newAddressPart,
        },
      };
    });
  };
  
  useEffect(() => {
    const headers = { Authorization: `Bearer ${token}` };
    Promise.all([
      fetch('http://localhost:8080/api/Dictionaries/skills', { headers, cache: 'no-store' }), // Dodano cache: no-store
      fetch('http://localhost:8080/api/Dictionaries/urlTypes', { headers, cache: 'no-store' }), // Dodano cache: no-store
    ]).then(async ([skillsRes, urlTypesRes]) => {
      const skillsData: Skill[] = await skillsRes.json();
      // setSkills(skillsData);
      setUrlTypes(await urlTypesRes.json());

      const grouped = skillsData.reduce((acc, skill) => {
        const typeName = skill.skillType?.name || 'Other';
        if (!acc[typeName]) {
          acc[typeName] = [];
        }
        acc[typeName].push(skill);
        return acc;
      }, {} as Record<string, Skill[]>);
      setGroupedSkills(grouped);
    }).catch(error => {
      console.error("Error fetching dictionary data:", error);
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

    try {
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
      } else {
        const errorText = await res.text();
        alert(`Failed to update profile: ${errorText}`);
      }
    } catch (error) {
      console.error("Error updating profile:", error);
      if(error instanceof Error)
      alert(`An unexpected error occurred: ${error.message}`);
    }
  };

  return (
    <InnerSection className="flex flex-col gap-4 mt-4 max-w-2xl">
      <label className="font-semibold text-gray-700 dark:text-gray-300"><b>Description</b></label>
      <textarea 
        className='global-field-style' 
        value={form.description} 
        onChange={(e) => setForm({ ...form, description: e.target.value })} 
      />

      <label className="font-semibold text-gray-700 dark:text-gray-300"><b>Contact Email</b></label>
      <input 
        className='global-field-style' 
        value={form.contactEmail} 
        onChange={(e) => setForm({ ...form, contactEmail: e.target.value })} 
        type="email"
     />

      <label className="font-semibold text-gray-700 dark:text-gray-300"><b>Phone Number</b></label>
      <input 
        className='global-field-style' 
        value={form.contactPhoneNumber} 
        onChange={(e) => setForm({ ...form, contactPhoneNumber: e.target.value })} 
        type="tel" // Upewnij się, że to jest typ tel
      />

      <label className="font-semibold text-gray-700 dark:text-gray-300"><b>Birth Date</b></label>
      <input 
        className='global-field-style' 
        type="date" 
        value={form.birthDate} 
        onChange={(e) => setForm({ ...form, birthDate: e.target.value })} 
      />

      {(form.address?.countryName != "") && (
        <div className="text-sm text-gray-700 dark:text-gray-300 italic mb-2">
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

      <div id="autocomplete-container" style={{ position: 'relative' }} className="mb-4" /> {/* Dodano mb-4 */}
      <label className="font-semibold text-gray-700 dark:text-gray-300">Apartment Number</label>
      <input
        type="text"
        className='global-field-style'
        value={form.address?.apartmentNumber || ''}
        onChange={(e) => setAddress({ ...form.address, apartmentNumber: e.target.value })}
      />
      
      <label className="flex items-center gap-2 text-gray-700 dark:text-gray-300">
        <input
          type="checkbox"
          checked={form.isTwoFactorAuthentication}
          onChange={(e) => setForm({ ...form, isTwoFactorAuthentication: e.target.checked })}
          className="form-checkbox h-4 w-4 text-blue-600 transition duration-150 ease-in-out rounded border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700"
        /> 
        Two-Factor Authentication
      </label>

      <label className="flex items-center gap-2 text-gray-700 dark:text-gray-300">
        <input
          type="checkbox"
          checked={form.isStudent}
          onChange={(e) => setForm({ ...form, isStudent: e.target.checked })}
          className="form-checkbox h-4 w-4 text-blue-600 transition duration-150 ease-in-out rounded border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700"
        /> 
        Are you a student?
      </label>

      {/* Sekcja wyboru umiejętności */}
      <h3 className='text-xl font-semibold text-gray-800 dark:text-gray-100 mt-4 mb-2'>Select Skills</h3>
      {Object.entries(groupedSkills).length > 0 ? (
        Object.entries(groupedSkills).map(([type, group]) => (
          <div key={type} className='mb-4'> {/* Key for the skill type group div */}
            <fieldset className='border border-gray-400 dark:border-gray-700 p-3 rounded-md'>
              <legend className='text-lg font-semibold text-gray-700 dark:text-gray-300 mb-2'>{type}</legend>
              <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-2 gap-3'>
                {group.map((skill: Skill) => { // 'skill' here is of type 'Skill'
                  const isSelected = form.skillsIds.includes(skill.skillId); // Sprawdzenie, czy umiejętność jest wybrana
                  
                  return (
                    <div key={skill.skillId} className='flex items-center justify-between'> {/* Usunięto border-gray-100, bo nie jest potrzebne */}
                      <label className='flex items-center gap-2 text-gray-700 dark:text-gray-300 cursor-pointer'>
                        <input
                          type='checkbox'
                          checked={isSelected}
                          onChange={(e) => handleSkillChange(skill.skillId, e.target.checked)}
                          className='form-checkbox h-5 w-5 text-blue-600 rounded-md border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700'
                        />
                        <span className='font-medium'>{skill.name}</span>
                      </label>
                    </div>
                  );
                })}
              </div>
            </fieldset>
          </div>
        ))
      ) : (
        <p className='text-gray-600 dark:text-gray-400 italic'>No skills available to select.</p>
      )}

      <label className="font-semibold text-gray-700 dark:text-gray-300"><b>Links</b></label>
      {form.urls.map((url, index) => (
        <div key={index} className="flex gap-2 items-center">
          <input 
            className='global-field-style flex-grow' 
            type="text" 
            value={url.value} 
            onChange={(e) => updateUrl(index, 'value', e.target.value)} 
          />
          <select 
            className='global-field-style' 
            value={url.urlTypeId} 
            onChange={(e) => updateUrl(index, 'urlTypeId', Number(e.target.value))}
          >
            {urlTypes.map(t => <option key={t.urlTypeId} value={t.urlTypeId}>{t.name}</option>)}
          </select>
          <button
            className='bg-red-600 text-white px-3 py-2 rounded-lg hover:bg-red-700 transition duration-300 ease-in-out shadow-md font-semibold'
            type="button" onClick={() => removeUrl(index)}>Remove</button>
        </div>
      ))}
      <button
        className='inline-block bg-green-600 text-white py-2 px-4 rounded-lg hover:bg-green-700 transition duration-300 ease-in-out shadow-md font-semibold self-start mt-2' // Dodano self-start i mt-2
        type="button" onClick={addUrl}>Add Link
      </button>

      <button
        onClick={handleSubmit}
        className="inline-block bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700 transition duration-300 ease-in-out shadow-md font-semibold self-start mt-4" // Dodano self-start i mt-4
      > 
        Update Profile
      </button>
    </InnerSection>
  );
};

export default RegularProfileForm;
